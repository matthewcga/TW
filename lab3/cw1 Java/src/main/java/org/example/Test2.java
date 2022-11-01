package org.example;

import java.util.concurrent.Semaphore;

import static org.example.Main.*;

public class Test2 {
    private static final Semaphore[] forks = new Semaphore[FORKS_COUNT];

    public Test2() { for (int i = 0; i < FORKS_COUNT; i++) forks[i] = new Semaphore(FORKS_COUNT, true); }

    public void test() {
        Philosopher2[] philosophers = new Philosopher2[PHIL_COUNT];
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i] = new Philosopher2(forks[i % FORKS_COUNT], forks[(i + 1) % FORKS_COUNT], i);
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i].start();
        for (int i = 0; i < PHIL_COUNT; i++) {
            try { philosophers[i].join(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        System.out.println(CCR + ">>>>>\t" + this.getClass().getSimpleName() + " finished");
        philosophers[1].Reset();
    }

    class Philosopher2 extends Philosopher {
        private final Semaphore leftFork, rightFork;
        public Philosopher2(Semaphore leftFork, Semaphore rightFork, int id)
        { super(id); this.leftFork = leftFork; this.rightFork = rightFork; }
        private void waitForAcquire() {
            while (!leftFork.isFair() && !rightFork.isFair())
                try { wait(); }
                catch (InterruptedException e) { HandleException(e); }
        }
        private void acquire(Semaphore fork) {
            try { fork.acquire(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        @Override
        protected void AcquireForks() { waitForAcquire(); acquire(leftFork); acquire(rightFork); }
        @Override
        protected void ReleaseForks() { leftFork.release(); rightFork.release(); }
    }
}
