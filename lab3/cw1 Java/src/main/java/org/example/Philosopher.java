package org.example;

import java.time.Duration;
import java.time.Instant;
import java.util.concurrent.ThreadLocalRandom;

import static org.example.Main.*;

public abstract class Philosopher extends Thread {
    private static final String[] COLORS = { "\u001B[31m", "\u001B[32m", "\u001B[33m", "\u001B[34m", "\u001B[35m", "\u001B[36m", "\u001B[37m" };
    private static boolean _someoneFinished = false;
    protected final int id;
    private int counter = 0;
    protected Philosopher(int id) { this.id = id; }
    private String printMsg(eAction action)
    { return COLORS[id % COLORS.length] + "P " + id + " " + action; }
    private void rndSleep() {
        try { Thread.sleep(ThreadLocalRandom.current().nextInt(MIN_SLEEP, MAX_SLEEP)); }
        catch (InterruptedException e) { HandleException(e); }
    }

    public void Reset() { _someoneFinished = false; }

    @Override
    public void run() {
        Instant start = Instant.now();
        while (++counter < MAX_ITER) {
            if (PRINT_EVENTS) System.out.println(printMsg(eAction.thinks));
            rndSleep();               // sleep
            AcquireForks();           // acquire forks from waiter
            if (PRINT_EVENTS) System.out.println(printMsg(eAction.eats));
            rndSleep();               // sleep
            ReleaseForks();           // release forks

            if (!PRINT_EVENTS && !_someoneFinished)
                System.out.print("\rprogress...\t" + String.format("%,.01f", (double) (counter * 100 / MAX_ITER)) + " %");
        }
        double avg = (double) Duration.between(start, Instant.now()).toMillis() / MAX_ITER;
        if (!_someoneFinished) { _someoneFinished = true; System.out.println(); }
        if (PRINT_AVG) System.out.println(printMsg(eAction.finished) + ", avg= " + String.format("%,.010f", avg) + " ms");
    }

    protected abstract void AcquireForks();

    protected abstract void ReleaseForks();
}
