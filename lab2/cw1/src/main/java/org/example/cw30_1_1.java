package org.example;

public class cw30_1_1 {
    public void test() {
        Buffer buf = new Buffer();
        Producer prod  = new Producer(buf);
        Consumer cons = new Consumer(buf);

        prod.start();
        cons.start();
    }

    class Producer extends Thread {
        private Buffer _buf;
        public void run() {
            for (int i = 0; i < 100; ++i)
                _buf.put(i);
        }
        public Producer(Buffer buf) { _buf = buf; }
    }

    class Consumer extends Thread {
        private Buffer _buf;
        public void run() {
            for (int i = 0; i < 100; ++i)
                System.out.println(_buf.get());
        }
        public Consumer(Buffer buf) { _buf = buf; }
    }

    class Buffer {
        private int val = 0;
        private boolean token = true;
        public synchronized void put(int i) {
            while (!token)
                try { wait(); }
                catch (InterruptedException e) { throw new RuntimeException(e); }
            val = i;
            token = false;
            notifyAll();
        }
        public synchronized int get() {
            while (token)
                try { wait(); }
                catch (InterruptedException e) { throw new RuntimeException(e); }
            token = true;
            notifyAll();
            return val;
        }
    }
}
