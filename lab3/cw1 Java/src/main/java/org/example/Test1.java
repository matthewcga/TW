package org.example;

import java.util.concurrent.Semaphore;

import static org.example.Main.*;

public class Test1 {
    private static final Semaphore[] forks = new Semaphore[FORKS_COUNT];

    public Test1() { for (int i = 0; i < FORKS_COUNT; i++) forks[i] = new Semaphore(FORKS_COUNT, true); }

    public void test() {
        Philosopher1[] philosophers = new Philosopher1[PHIL_COUNT];
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i] = new Philosopher1(forks[i % FORKS_COUNT], forks[(i + 1) % FORKS_COUNT], i);
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i].start();
        for (int i = 0; i < PHIL_COUNT; i++) {
            try { philosophers[i].join(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        System.out.println(CCR + ">>>>>\t" + this.getClass().getSimpleName() + " finished");
        philosophers[1].Reset();
    }

    class Philosopher1 extends Philosopher {
        private final Semaphore leftFork, rightFork;
        public Philosopher1(Semaphore leftFork, Semaphore rightFork, int id)
        { super(id); this.leftFork = leftFork; this.rightFork = rightFork; }
        private void acquire(Semaphore fork) {
            try { fork.acquire(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        @Override
        protected void AcquireForks() { acquire(leftFork); acquire(rightFork); }
        @Override
        protected void ReleaseForks() { leftFork.release(); rightFork.release(); }
    }
}