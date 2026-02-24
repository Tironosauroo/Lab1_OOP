using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Unit tests for the generic InventoryQueue class.
/// Verifies the core logic of the circular buffer implementation.
/// </summary>
public class InventoryQueueTest
{
    /// <summary>
    /// Tests if elements are successfully added to the queue in the correct order.
    /// </summary>
    [Test]
    public void Enqueue_AddsElementsInOrder()
    {
        var queue = new InventoryQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        var arr = queue.IQtoArray();

        Assert.AreEqual(new[] { 1, 2, 3 }, arr);
        Assert.AreEqual(3, queue.Count);
    }

    /// <summary>
    /// Tests if the Dequeue method correctly removes and returns the first element.
    /// </summary>
    [Test]
    public void Dequeue_RemovesFirstElement()
    {
        var queue = new InventoryQueue<string>();
        queue.Enqueue("apple");
        queue.Enqueue("banana");
        queue.Enqueue("cherry");

        string first = queue.Dequeue();

        Assert.AreEqual("apple", first);
        Assert.AreEqual(2, queue.Count);

        var arr = queue.IQtoArray();
        Assert.AreEqual(new[] { "banana", "cherry" }, arr);
    }

    /// <summary>
    /// Ensures that attempting to dequeue from an empty queue throws an InvalidOperationException.
    /// </summary>
    [Test]
    public void Dequeue_EmptyQueue_ThrowsException()
    {
        var queue = new InventoryQueue<int>();
        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    /// <summary>
    /// Verifies that the queue correctly doubles its capacity when full, while preserving element order.
    /// </summary>
    [Test]
    public void Resize_ExpandsCapacityAndPreservesOrder()
    {
        var queue = new InventoryQueue<int>(2);

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3); // Resize() triggers here

        Assert.AreEqual(3, queue.Count);
        var arr = queue.IQtoArray();
        Assert.AreEqual(new[] { 1, 2, 3 }, arr);
    }

    /// <summary>
    /// Checks if dequeuing an element properly clears the reference from the array.
    /// </summary>
    [Test]
    public void Dequeue_ClearsReference()
    {
        var queue = new InventoryQueue<string>();
        queue.Enqueue("Sword");
        queue.Enqueue("Shield");

        queue.Dequeue(); // delete "Sword"

        var arr = queue.IQtoArray();

        Assert.AreEqual(1, arr.Length);
        Assert.AreEqual("Shield", arr[0]);
    }

    /// <summary>
    /// Validates the circular nature of the buffer by performing multiple enqueues and dequeues.
    /// </summary>
    [Test]
    public void EnqueueDequeueMultipleTimes_WorksCorrectly()
    {
        var queue = new InventoryQueue<int>(3);

        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);
        queue.Dequeue(); // delete 10
        queue.Enqueue(40); // wrap around

        var arr = queue.IQtoArray();
        Assert.AreEqual(new[] { 20, 30, 40 }, arr);
    }
}