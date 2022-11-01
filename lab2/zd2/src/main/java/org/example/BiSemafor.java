package org.example;

public class BiSemafor implements iSemafor {
    private boolean _czeka;

    public BiSemafor(boolean i) {
        _czeka = i;
    }

    public synchronized void V() {
        while (_czeka)
            try {
                this.wait();
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
        _czeka = true;
        notifyAll();
    }

    public synchronized void P() {
        while (!_czeka)
            try {
                this.wait();
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
        _czeka = false;
        notifyAll();
    }
}
