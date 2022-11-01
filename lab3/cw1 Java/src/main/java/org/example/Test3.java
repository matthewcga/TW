package org.example;

import java.util.concurrent.Semaphore;

import static org.example.Main.*;

public class Test3 {
    private static final Semaphore[] forks = new Semaphore[FORKS_COUNT];

    public Test3() { for (int i = 0; i < FORKS_COUNT; i++) forks[i] = new Semaphore(FORKS_COUNT, true); }

    public void test() {
        Philosopher3[] philosophers = new Philosopher3[PHIL_COUNT];
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i] = new Philosopher3(forks[i % FORKS_COUNT], forks[(i + 1) % FORKS_COUNT], i);
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i].start();
        for (int i = 0; i < PHIL_COUNT; i++) {
            try { philosophers[i].join(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        System.out.println(CCR + ">>>>>\t" + this.getClass().getSimpleName() + " finished");
        philosophers[1].Reset();
    }

    class Philosopher3 extends Philosopher {
        private final int firstForkIndex, secondForkIndex;
        private final Semaphore[] forks = new Semaphore[2];
        public Philosopher3(Semaphore leftFork, Semaphore rightFork, int id) {
            super(id); boolean isEven = id % 2 == 0;
            forks[0] = leftFork; forks[1] = rightFork;
            firstForkIndex = isEven ? 0 : 1; secondForkIndex = !isEven ? 0 : 1;
        }
        private void acquire(Semaphore fork) {
            try { fork.acquire(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        @Override
        protected void AcquireForks() { acquire(forks[firstForkIndex]); acquire(forks[secondForkIndex]); }
        @Override
        protected void ReleaseForks() { forks[firstForkIndex].release(); forks[secondForkIndex].release();}
    }
}
