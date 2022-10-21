package org.example;

public class ZepsutySemafor implements iSemafor {
    private boolean _czeka;

    public ZepsutySemafor(boolean i) {
        _czeka = i;
    }

    public synchronized void V() {
        if (_czeka)
            try {
                this.wait();
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
        _czeka = true;
        notifyAll();
    }

    public synchronized void P() {
        if (!_czeka)
            try {
                this.wait();
            } catch (InterruptedException ex) {
                ex.printStackTrace();
            }
        _czeka = false;
        notifyAll();
    }
}
