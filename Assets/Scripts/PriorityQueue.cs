using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<Tuple<T, float>> list = new List<Tuple<T, float>>();
    private HashSet<T> set = new HashSet<T>();

    private bool requireSort = false;

    public int Count
    {
        get => list.Count;
    }

    public bool IsEmpty()
    {
        return list.Count == 0;
    }

    public bool Contains(T item)
    {
        return set.Contains(item);
    }

    public void Enqueue(T item, float score)
    {
        list.Add(Tuple.Create(item, score));
        set.Add(item);
        requireSort = true;
    }

    public void Replace(T item, float score)
    {
        Debug.Assert(set.Contains(item));

        var index = list.FindIndex(delegate (Tuple<T, float> t)
        {
            return t.Item1.Equals(t);
        });

        list[index] = Tuple.Create(item, score);
    }

    public T Dequeue()
    {
        SortList();

        if (!IsEmpty())
        {
            var item = list[0].Item1;
            list.RemoveAt(0);
            set.Remove(item);
            return item ;
        }

        return default;
    }

    private void SortList()
    {
        if (!requireSort)
        {
            return;
        }

        list.Sort(delegate (Tuple<T, float> a, Tuple<T, float> b) {
            return a.Item2.CompareTo(b.Item2);
        });
        requireSort = false;

    }
}

