using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : CustomScriptableObject, IEnumerable<T>
{
    [SerializeField] private List<T> _items = new List<T>();

    public Action<T> OnItemAdded;
    public Action<T> OnItemRemoved;

    public List<T> Items {
        get { return _items; }
    }

    public override void Reset()
    {
        OnItemAdded = null;
        OnItemRemoved = null;
        _items = new List<T>();
    }

    public void Add(T item)
    {
        _items.Add(item);
        OnItemAdded?.Invoke(item);
    }
    public void AddRange(IEnumerable<T> collection)
    {
        _items.AddRange(collection);
    }
    public void Remove(T item)
    {
        _items.Remove(item);
        OnItemRemoved?.Invoke(item);
    }

    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }
    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }
    public T this[int i] {
        get { return _items[i]; }
        set { _items[i] = value; }
    }
    public int Count
    {
        get { return _items.Count; }
    }
}