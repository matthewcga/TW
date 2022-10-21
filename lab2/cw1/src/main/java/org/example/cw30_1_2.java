package org.example;

public class cw30_1_2 {
    public void test(int p, int c) {
        Buffer buf = new Buffer(p, c);

        Producer[] prod = new Producer[p];
        for (int i = 0; i < p; i++) prod[i] = new Producer(buf, i);
        Consumer[] cons = new Consumer[c];
        for (int i = 0; i < c; i++) cons[i] = new Consumer(buf, i);

        for (int i = 0; i < p; i++) prod[i].start();
        for (int i = 0; i < c; i++) cons[i].start();
    }

    class Producer extends Thread {
        private Buffer _buf;
        private final int id;
        public void run() {
            for (int i = 0; i < 100; ++i)
                _buf.put(id, i);
                //try {  _buf.put(id, i); sleep(250); }
                //catch (InterruptedException e) { throw new RuntimeException(e); }
        }
        public Producer(Buffer buf, int i) { _buf = buf; id = i; }
    }

    class Consumer extends Thread {
        private Buffer _buf;
        private final int id;
        public void run() {
            for (int i = 0; i < 100; ++i)
                System.out.println(_buf.get(id));
                //try { System.out.println("cid:" + id + " - " + _buf.get(id)); sleep(250); }
                //catch (InterruptedException e) { throw new RuntimeException(e); }
        }
        public Consumer(Buffer buf, int i) { _buf = buf; id = i; }
    }

    class Buffer {
        private int val = 0;
        private boolean token = true;
        private int pid = 0, cid = 0;
        private final int pmax, cmax;

        public Buffer(int p, int c) { pmax = p; cmax = c;}

        public synchronized void put(int id, int i) {
            while (!token && id != pid)
                try {
                    wait();
                } catch (InterruptedException e) {
                    throw new RuntimeException(e);
                }
            val = i;
            token = false;
            pid = (pid + 1) % pmax;
            //System.out.println("put call from " + id);
            notifyAll();
        }

        public synchronized int get(int id) {
            while (token && cid != id)
                try {
                    wait();
                } catch (InterruptedException e) {
                    throw new RuntimeException(e);
                }
            token = true;
            cid = (cid + 1) % cmax;
            System.out.println("get call from " + id);
            notifyAll();
            return val;
        }
    }
}
