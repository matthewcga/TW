package org.example;

public class Main {
    // SETTINGS # # # # # # # # # # # #
    public static final boolean PRINT_AVG = false, PRINT_EVENTS = true; // wypisuje średnią czasów cyklu po wszystkich iteracjach, wypisuje zdarzenia (kto je a kto myśli)
    public static final int PHIL_COUNT = 5, MIN_SLEEP = 50, MAX_SLEEP = 51, MAX_ITER = 10; // liczba filozofów, min/max czas myślenia jedzenia, ilośc cykli
    // SETTINGS # # # # # # # # # # # #
    public static final int FORKS_COUNT = PHIL_COUNT - 1;
    public static final String CCR = "\u001B[0m";

    public static void main(String[] args) {
        //new Test1().test(); // zad 1
        //new Test2().test(); // zad 2
        new Test3().test(); // zad 3
        //new Test4().test(); // zad 4
    }

    public static <T extends Exception> void HandleException(T e) { e.printStackTrace(); System.exit(99); }
}