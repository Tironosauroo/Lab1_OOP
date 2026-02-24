using System;

/// <summary>
/// Implementing a circular buffer for storing objects.
/// </summary>
/// <remarks>
/// <para>The Enqueue method algorithm is described in UML: <a href="ActivityDiagram.png">Activity diagram</a>.</para>
/// </remarks>
/// <typeparam name="T">The type of item in the queue.</typeparam>
public class InventoryQueue<T> : IInventoryQueue<T>
{
    /// <summary>Internal array for storing elements.</summary>
    private T[] _array;
    /// <summary>The index of the first element in the queue (the head).</summary>
    private int _head;
    /// <summary>The index of the next free space in the queue (tail).</summary>
    private int _tail;
    /// <summary>The current number of items in the queue.</summary>
    private int _size;
    /// <summary>The current maximum capacity of the array.</summary>
    private int _capacity;

    /// <summary>Initializes the queue with an initial capacity.</summary>
    /// <param name="capacity">Initial size of the internal array (default 4).</param>
    public InventoryQueue(int capacity = 4)
    {
        _capacity = capacity;
        _array = new T[_capacity];
        _head = 0;
        _tail = 0;
        _size = 0;
    }

    /// <summary>Gets the current number of items in the queue.</summary>
    public int Count => _size;

    /// <summary>Adds a new element to the end of the queue. If the array is full, Resize() is called.</summary>
    /// <param name="item">Item to add.</param>
    public void Enqueue(T item)
    {
        if (_size == _capacity)
            Resize();

        _array[_tail] = item;
        _tail = (_tail + 1) % _capacity;
        _size++;
    }

    /// <summary>Removes and returns the first element from the queue.</summary>
    /// <returns>An element of type T that was at the beginning of the queue.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the queue is empty.</exception>
    public T Dequeue()
    {
        if (_size == 0)
            throw new InvalidOperationException("Queue is empty");

        T item = _array[_head];
        _array[_head] = default(T);
        _head = (_head + 1) % _capacity;
        _size--;
        return item;
    }

    /// <summary>Returns all elements of the queue as a standard array with the correct order.</summary>
    /// <returns>An array of elements of type T.</returns>
    public T[] IQtoArray()
    {
        T[] result = new T[_size];
        for (int i = 0; i < _size; i++)
        {
            result[i] = _array[(_head + i) % _capacity];
        }
        return result;
    }

    /// <summary>
    /// Dynamically doubles the size of an array when it overflows, while maintaining the correct order of elements.
    /// </summary>
    private void Resize()
    {
        int newCapacity = _capacity * 2;
        T[] newArray = new T[newCapacity];

        for (int i = 0; i < _size; i++)
            newArray[i] = _array[(_head + i) % _capacity];

        for (int i = 0; i < _capacity; i++)
            _array[i] = default(T);

        _array = newArray;
        _head = 0;
        _tail = _size;
        _capacity = newCapacity;
    }
}