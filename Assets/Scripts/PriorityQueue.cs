using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<Tuple<T, float>> _list = new();
    private HashSet<T> _set = new();
    private bool _requireSort = false;

    public int Count
    {
        get => _list.Count;
    }

    public bool IsEmpty()
    {
        return _list.Count == 0;
    }

    public bool Contains(T item)
    {
        return _set.Contains(item);
    }

    public void Enqueue(T item, float score)
    {
        _list.Add(Tuple.Create(item, score));
        _set.Add(item);
        _requireSort = true;
    }

    public void Replace(T item, float score)
    {
        Debug.Assert(_set.Contains(item));

        var index = _list.FindIndex(delegate (Tuple<T, float> t)
        {
            return t.Item1.Equals(item);
        });

        Debug.Log($"PriorityQueue.Replace: {item}, score: {score}, index: {index}, size: {_list.Count}, contains: {_set.Contains(item)}");

        _list[index] = Tuple.Create(item, score);
    }

    public T Dequeue()
    {
        SortList();

        if (!IsEmpty())
        {
            var item = _list[0].Item1;
            _list.RemoveAt(0);
            _set.Remove(item);
            return item ;
        }

        return default;
    }

    private void SortList()
    {
        if (!_requireSort)
        {
            return;
        }

        _list.Sort(delegate (Tuple<T, float> a, Tuple<T, float> b) {
            return a.Item2.CompareTo(b.Item2);
        });
        _requireSort = false;

    }
}

