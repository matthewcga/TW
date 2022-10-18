package org.example;

public class Main {
    private static final int loops = 1000000, tests = 10;
    private static final long sleepTime = 60000;

    public static boolean token = true;

    public static void main(String[] args) {
        for (int i = 0; i < tests; i++) Test();
        for (int i = 0; i < tests; i++) TestSynced();

        CounterSync.resetI();

        try
        {
            while (token)
            {
                new Thread () {
                    public void run () {
                        try { Thread.sleep(sleepTime); }
                        catch (InterruptedException ex) { token = false; System.out.println(ex); }
                    }
                }.start();
                CounterSync.updateI(true);
                System.out.format("\r%d", CounterSync.getI());
            }
        }
        catch (Exception ex)
        {
            if (!token) System.out.println("Runtime Exception!");
            System.out.println("\nException was thrown at: " + CounterSync.getI() + "\n" + ex);
        }
    }

    private static void Test() {
        StopWatch sw = new StopWatch();
        sw.start();

        Thread t1 = new Thread () { public void run () { for (int i = 0; i < loops; i++) Counter.updateI(true); } };
        Thread t2 = new Thread () { public void run () { for (int i = 0; i < loops; i++) Counter.updateI(false); } };

        t1.start(); t2.start();

        try { t1.join(); t2.join(); }
        catch (InterruptedException ex) { System.out.println(ex); }

        sw.stop();

        System.out.println(Counter.class.getSimpleName() + ":\t" + sw.getElapsedTime() + "ms,\tvalue = " + Counter.getI());
        Counter.resetI();
    }

    private static void TestSynced() {
        StopWatch sw = new StopWatch();
        sw.start();

        Thread t1 = new Thread () { public synchronized void run () { for (int i = 0; i < loops; i++) CounterSync.updateI(true); } };
        Thread t2 = new Thread () { public synchronized void run () { for (int i = 0; i < loops; i++) CounterSync.updateI(false); } };

        t1.start(); t2.start();

        try { t1.join(); t2.join(); }
        catch (InterruptedException ex) { System.out.println(ex); }

        sw.stop();

        System.out.println(CounterSync.class.getSimpleName() + ":\t" + sw.getElapsedTime() + "ms,\tvalue = " + CounterSync.getI());
        CounterSync.resetI();
    }
}

class Counter {
    private static int i = 0;
    public static int getI() { return i; }
    public static void updateI(boolean inc) { i += inc ? 1 : -1; }
    public static void resetI() { i = 0; }
}

class CounterSync {
    private static int i = 0;
    public static int getI() { return i; }
    public static synchronized void updateI(boolean inc) { i += inc ? 1 : -1; }
    public static void resetI() { i = 0; }
}