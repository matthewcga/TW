const Fork = function() {
    this.state = 0;
    return this;
};

Fork.prototype.acquire = function(fn) {
    const fork = this,
        runFork = function(waitTime) {
            setTimeout(function() {
                if (fork.state === 0) {
                    fork.state = 1;
                    fn();
                } else runFork(waitTime * 2);
            }, waitTime);
        };
    runFork(1);
};

Fork.prototype.release = function() {
    this.state = 0;
};

const Philosopher = function(id, forks) {
    this.id = id;
    this.forks = forks;
    this.leftFork = id % forks.length;
    this.rightFork = (id + 1) % forks.length;
    this.timer = 0;
    this.timerCounter = -1;
    return this;
};

let fn1, fn2, counter, i;

Philosopher.prototype.startAsymmetric = function(count) {
    this.timer = 0;
    const forks = this.forks,
        leftFork = this.id % 2 === 0 ? this.leftFork : this.rightFork,
        rightFork = this.id % 2 === 0 ? this.rightFork : this.leftFork,
        myself = this,
        runAsymmetric = function(count) {
            myself.timerCounter = new Date().getTime();
            if (count > 0)
                forks[leftFork].acquire(function() {
                    forks[rightFork].acquire(function() {
                        myself.timer += new Date().getTime() - myself.timerCounter;
                        setTimeout(
                            function() { forks[leftFork].release(); forks[rightFork].release(); runAsymmetric(count - 1);},
                            1
                        );
                    });
                });
            else { --counter; if (counter === 0) fn2(); }
        };
    setTimeout(function() { runAsymmetric(count);}, 1);
};

Philosopher.prototype.startWaiter = function(count, waiter) {
    this.timer = 0;
    const forks = this.forks,
        leftFork = this.leftFork,
        rightFork = this.rightFork,
        id = this.id,
        myself = this,
        runWaiter = function(count) {
            myself.timerCounter = new Date().getTime();
            if (count > 0)
                waiter.requestForks(id, function () {
                    forks[leftFork].acquire(function () {
                        forks[rightFork].acquire(function () {
                            myself.timer += new Date().getTime() - myself.timerCounter;
                            setTimeout(
                                function () {
                                    forks[leftFork].release();
                                    forks[rightFork].release();
                                    waiter.returnForks(id, function () {runWaiter(count - 1);})
                                    ;},
                                1);
                        });
                    });
                });
            else {--counter; if (counter === 0) TestResults3();}
        };
    setTimeout(function() {runWaiter(count);}, 1);
};

function acquireNaive(leftFork, rightFork, action) {
    const fn = function(waitTime) {
        setTimeout(function() {
            if (leftFork.state === 0 && rightFork.state === 0) {
                leftFork.state = rightFork.state = 1;
                action();
            } else fn(waitTime * 2);
        }, waitTime);
    }; fn(1);
}

Philosopher.prototype.startNaive = function(count) {
    this.timer = 0;
    const forks = this.forks,
        leftFork = this.leftFork,
        rightFork = this.rightFork,
        myself = this,
        runNaive = function(count) {
            myself.timerCounter = new Date().getTime();
            if (count > 0)
                acquireNaive(forks[leftFork], forks[rightFork], function() {
                    myself.timer += new Date().getTime() - myself.timerCounter;
                    setTimeout(function() {
                        forks[leftFork].release();
                        forks[rightFork].release();
                        runNaive(count - 1);
                    }, 1);
                });
            else {--counter; if (counter === 0) fn1();}
        }; setTimeout(function() { runNaive(count); }, 1);
};

Waiter = function() {
    this.philosophersAtTable = [];
    this.waitingCallbacks = [];
    this.waitingPhilosophers = [];
    for (let i = 0; i < PHIL_COUNT; i++) this.philosophersAtTable.push(0);
    return this;
};

Waiter.prototype.tableCanAccess = function() {
    let count = 0,
        prev = this.philosophersAtTable[PHIL_COUNT - 1],
        neighbours = false;
    for (let i = 0; i < PHIL_COUNT; i++)
        if (this.philosophersAtTable[i]) {
            if (++count > 2) return false;
            if (prev) neighbours = true;
            prev = true;
        } else prev = false;
    return count < 2 || neighbours;
};

Waiter.prototype.requestForks = function (philosopherId, fn) {
    if (this.tableCanAccess()) {
        this.philosophersAtTable[philosopherId] = 1;
        setTimeout(fn);
    } else {
        this.waitingCallbacks.unshift(fn);
        this.waitingPhilosophers.unshift(philosopherId);
    }
};

Waiter.prototype.returnForks = function(philosopherId, cb) {
    this.philosophersAtTable[philosopherId] = 0;
    while (this.waitingCallbacks.length > 0 && this.tableCanAccess()) {
        const philosopherIdFromList = this.waitingPhilosophers.pop(),
            cbFromList = this.waitingCallbacks.pop();
        this.philosophersAtTable[philosopherIdFromList] = 1;
        setTimeout(cbFromList);
    }setTimeout(cb);
};

function Results() { for (i = 0; i < PHIL_COUNT; i++) console.log(philosophers[i].timer / ITER_COUNT); }
function TestResults1() { Results(); console.log(">>>> Test podejście Naiwne\n"); }
function TestResults2() { Results(); console.log(">>>> Test podejście Asymetryczne\n"); }
function TestResults3() { Results(); console.log(">>>> Test podejście z Kelnerem\n"); }

const PHIL_COUNT = 5, ITER_COUNT = 10;
const forks = [], philosophers = [], waiter = new Waiter();

for (i = 0; i < PHIL_COUNT; i++) forks.push(new Fork());
for (i = 0; i < PHIL_COUNT; i++) philosophers.push(new Philosopher(i, forks));

counter = PHIL_COUNT;
for (i = 0; i < PHIL_COUNT; i++) philosophers[i].startNaive(ITER_COUNT);

fn1 = function() {
    TestResults1();
    counter = PHIL_COUNT;
    for (i = 0; i < PHIL_COUNT; i++) philosophers[i].startAsymmetric(ITER_COUNT);
};

fn2 = function() {
    TestResults2();
    counter = PHIL_COUNT;
    for (i = 0; i < PHIL_COUNT; i++) philosophers[i].startWaiter(ITER_COUNT, waiter);
};