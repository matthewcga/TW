package org.example;

public class Main {
    public static void main(String[] args) {
        LockList<String> list = new LockList<>();

        System.out.println(list);

        list.add("object1");
        list.add("object2");
        list.add("object3");
        list.add("object4");
        list.add("object5");

        System.out.println(list);

        list.remove("object1");
        list.remove("object3");
        list.remove("object5");

        System.out.println(list);

        System.out.println("'object2' " + (list.contains("object2") ? "'IS'" : "'IS NOT'") + " in list");
        System.out.println("'object7' " + (list.contains("object7") ? "'IS'" : "'IS NOT'") + " in list");
    }
}