package org.example;

import java.util.concurrent.Semaphore;

import static org.example.Main.*;

public class Test4{
    private Waiter waiter = new Waiter();

    public void test() {
        Philosopher4[] philosophers = new Philosopher4[PHIL_COUNT];
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i] = new Philosopher4(waiter, i);
        for (int i = 0; i < PHIL_COUNT; i++)
            philosophers[i].start();
        for (int i = 0; i < PHIL_COUNT; i++) {
            try { philosophers[i].join(); }
            catch (InterruptedException e) { HandleException(e); }
        }
        System.out.println(CCR + ">>>>>\t" + this.getClass().getSimpleName() + " finished");
        philosophers[1].Reset();
    }

    class Waiter {
        private final boolean[] table = new boolean[PHIL_COUNT];
        private final Semaphore tableAccess = new Semaphore(1, true), waiterBusy = new Semaphore(1, true);
        private boolean tableCanAccess() {
            int count = 0; boolean prev = table[PHIL_COUNT - 1], neighbours = false;
            for (int i = 0; i < PHIL_COUNT; i++)
                if (table[i]) {
                    if (++count > 2) return false;
                    if (prev) neighbours = true;
                    prev = true;
                } else prev = false;
            return count < 2 || neighbours;
        }
        public void RequestForks(int id){
            try { tableAccess.acquire(); waiterBusy.acquire(); }
            catch (InterruptedException e) { HandleException(e); }
            table[id] = true;
            if (tableCanAccess()) tableAccess.release();
            waiterBusy.release();
        }
        public void ReturnForks(int id){
            try { waiterBusy.acquire(); }
            catch (InterruptedException e) { HandleException(e); }
            table[id] = false;
            if (tableCanAccess()) tableAccess.release();
            waiterBusy.release();
        }
    }

    class Philosopher4 extends Philosopher {
        private final Waiter waiter;
        public Philosopher4(Waiter waiter, int id) { super(id); this.waiter = waiter; }
        @Override
        protected void AcquireForks() { waiter.RequestForks(id); }
        @Override
        protected void ReleaseForks() { waiter.ReturnForks(id); }
    }
}
