package org.example;

public class Main {
    public static void main(String[] args) throws InterruptedException {
        //new cw30_1_1().test(); // wait, notify, 1 producent, 1 konsument
        //new cw30_1_2().test(5, 6); // wait, notify, x producentów, y konsumentów (sleep testy wykomentowane)
        //new cw30_2_1().test(); // semafor, 1 producent, 1 konsument
        new cw30_2_2().test(5, 6); // semafor, x producentów, y konsumentów
    }
}
