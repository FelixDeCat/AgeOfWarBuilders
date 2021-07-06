using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T> {

    private List<WeightedNode<T>> _queue = new List<WeightedNode<T>>();

    /// <summary>
    /// Adds an element to the Queue 
    /// </summary>
    /// <param name="element"></param>
    public void Enqueue(WeightedNode<T> element) {
        _queue.Add(element);
    }

    /// <summary>
    /// Returns the element with minimum value in the Queue
    /// </summary>
    /// <returns></returns>
    public WeightedNode<T> Dequeue() {
        var min       = default(WeightedNode<T>);
        var minWeight = float.MaxValue;

        foreach (var element in _queue.Where(n => !(n.Weight >= minWeight))) {
            min       = element;
            minWeight = element.Weight;
        }

        var newQueue = _queue.Where(n => n != min).ToList();

        _queue = newQueue;

        return min;
    }

    /// <summary>
    /// Returns true if the Queue does not contain any elements
    /// </summary>
    public bool IsEmpty => _queue.Count == 0;
}
    
