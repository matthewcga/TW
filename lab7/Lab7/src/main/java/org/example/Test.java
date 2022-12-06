package org.example;

import java.util.Random;

public class Test {
    private final LockList<String> list = new LockList<>();

    private Thread GetThread(int loops) {
        return new Thread() { public void run () {
            Random rnd = new Random();
            int dice;
            for (int i = 0; i < loops; i++) {
                dice = rnd.nextInt(10);
                String item = String.valueOf(rnd.nextInt(5));
                switch (dice) {
                    case 1, 2, 3 : System.out.println("adding " + item + list.add(item)); break;
                    case 4, 5, 6 : System.out.println("is " + item + "in list?" + list.contains(item)); break;
                    case 7, 8, 9 : System.out.println("removing " + item + "from list" + list.remove(item)); break;
                }
            }
        }};
    }

    public Test(int count, int loops) {
        Thread[] threads = new Thread[count];
        for (int i = 0; i < count; i++)
            threads[i] = GetThread(loops);
        for (int i = 0; i < count; i++)
            threads[i].start();
        for (int i = 0; i < count; i++) {
            try {
                threads[i].join();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }
    }
}
