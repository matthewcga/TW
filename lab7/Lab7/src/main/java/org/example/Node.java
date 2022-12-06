package org.example;

public class Node<T> {
    public boolean Lock;
    public final Object Value;
    public Node<T> Next;

    public Node(T value) {
        Value = value;
        Lock = false;
        Next = null;
    }


}
