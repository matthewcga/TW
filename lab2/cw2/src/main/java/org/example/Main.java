package org.example;

public class Main {
    public static final String ANSI_YELLOW = "\u001B[33m", ANSI_RED = "\u001B[31m", ANSI_GREEN = "\u001B[32m";
    public static final int LOOPS = 100_000, N = 4;

    public static void main(String[] args) {
        System.out.println(ANSI_YELLOW + "binarny semafor test:");
        test(new BiSemafor(true), N, LOOPS); // semafor, wątki, iteracji

        System.out.println(ANSI_YELLOW + "\nsemafor licznikowy test:");
        test(new Semafor(1), N, LOOPS);

        System.out.println(ANSI_YELLOW + "\nniedziałajcy semafor test:");
        test(new ZepsutySemafor(true), N, LOOPS);
    }

    public static void test(iSemafor sem, int n, int l) {
        WyscigTest test = new WyscigTest(sem);
        Thread inc[] = new Thread[n], dec[] = new Thread[n];

        for (int i = 0; i < n; i++) {
            inc[i] = new Thread(() -> { for (int j = 0; j < l; j++) test.inc(); });
            dec[i] = new Thread(() -> { for (int j = 0; j < l; j++) test.dec(); });
        }
        for (int i = 0; i < n; i++) { inc[i].start(); dec[i].start(); }
        try { for (int i = 0; i < n; i++) { inc[i].join(); dec[i].join(); } }
        catch (InterruptedException ex) { ex.printStackTrace(); }

        String color = test.getValue() == 0 ? ANSI_GREEN : ANSI_RED;
        System.out.println(color + "result: " + test.getValue());
    }

    static class WyscigTest {
        private int val = 0;
        private final iSemafor sem;
        public WyscigTest(iSemafor sem) { this.sem = sem; }
        public void inc() { sem.P(); val++; sem.V(); }
        public void dec() { sem.P(); val--; sem.V(); }
        public int getValue() { return val; }
    }
}