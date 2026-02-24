/// <summary>
/// An interface for abstracting inventory queue logic.
/// Uses Generics for flexibility of object types.
/// </summary>
/// <typeparam name="T">The type of items to be stored in the queue.</typeparam>
public interface IInventoryQueue<T>
{
    /// <summary>The current number of items in the queue.</summary>
    int Count { get; }

    /// <summary>Adds a new element to the end of the queue.</summary>
    /// <param name="item">Item to add.</param>
    void Enqueue(T item);

    /// <summary>Removes and returns an element from the beginning of the queue.</summary>
    /// <returns>Перший елемент у черзі.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown if the queue is empty.</exception>
    T Dequeue();

    /// <summary>Converts the current queue state into an array.</summary>
    /// <returns>An array of elements in the correct order (from head to tail).</returns>
    T[] IQtoArray();
}