package org.example;

public class cw30_2_1 {
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
        public Producer(Buffer buf) {_buf = buf;}
    }

    class Consumer extends Thread {
        private Buffer _buf;
        public void run() {
            for (int i = 0; i < 100; ++i)
                _buf.get();
        }
        public Consumer(Buffer buf) {_buf = buf;}
    }

    class Buffer {
        private int val = 0;
        private Semafor putSem = new Semafor(1), getSem = new Semafor(0);

        public void put(int i) {
            putSem.P();
            val = i;
            getSem.V();
        }

        public void get() {
            getSem.P();
            System.out.println(val);
            putSem.V();
        }
    }

    class Semafor {
        private int _czeka;
        public Semafor(int i) { _czeka = i; }
        public synchronized void V() {
            _czeka++;
            notifyAll();
        }
        public synchronized void P() {
            while (_czeka == 0)
                try {this.wait();}
                catch (InterruptedException ex) {ex.printStackTrace();}
            _czeka--;
            notifyAll();
        }
    }
}
