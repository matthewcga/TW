package org.example;

public class LockList<T> {
    private Node<T> First;

    public LockList() {
        First = null;
    }

    public boolean contains(T value) {
        if (First == null)
            return false;

        First.Lock = true;
        if (First.Value == value)
            return true;

        Node<T> current = First.Next, previous = First;

        while (current != null) {
            current.Lock = true;
            if (current.Value == value){
                current.Lock = false;
                previous.Lock = false;
                return true;
            }
            previous.Lock = false;
            previous = current;
            current = current.Next;
        }

        previous.Lock = false;
        return false;
    }

    public boolean remove(T value) {
        if (First == null)
            return false;

        First.Lock = true;
        if (First.Value == value) {
            First = First.Next;
            return true;
        }

        Node<T> current = First.Next, previous = First;

        while (current != null) {
            current.Lock = true;
            if (current.Value == value) {
                previous.Next = current.Next;
                previous.Lock = false;
                return true;
            }

            previous.Lock = false;
            previous = current;
            current = current.Next;
        }

        previous.Lock = false;
        return false;
    }

    public boolean add(T value) {
        Node<T> newNode = new Node<>(value);

        if (First == null) {
            First = newNode;
            return true;
        }

        First.Lock = true;
        Node<T> current = First.Next, previous = First;

        while (current != null) {
            current.Lock = true;
            previous.Lock = false;
            previous = current;
            current = current.Next;
        }

        previous.Next = newNode;
        previous.Lock = false;
        return true;
    }

    @Override
    public String toString() {
        if (First == null)
            return "*";

        First.Lock = true;
        StringBuilder sb = new StringBuilder("head -> " + First.Value + " -> ");
        Node<T> current = First.Next, previous = First;

        while (current != null) {
            current.Lock = true;
            sb.append(current.Value.toString()).append(" -> ");
            previous.Lock = false;
            previous = current;
            current = current.Next;
        }

        previous.Lock = false;
        return  sb + "*";
    }
}
