package org.example;

public class Semafor implements iSemafor {
    private int _czeka;
    private final BiSemafor canRelSem, canAcqSem = new BiSemafor(true);
    public Semafor(int i) { _czeka = i; canRelSem = new BiSemafor(i > 0); }

    public void V() {
        canAcqSem.P();
        _czeka++;
        if (_czeka == 1) canRelSem.V();
        canAcqSem.V();
    }

    public void P() {
        canRelSem.P();
        canAcqSem.P();
        _czeka--;
        if (_czeka > 0) canRelSem.V();
        canAcqSem.V();
    }

    public int getPermits() {
        canAcqSem.P();
        int perm = _czeka;
        canAcqSem.V();
        return perm;
    }
}
