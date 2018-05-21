using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {

  T[] items;
  int currentItemCount;
  public int Count {
    get {
      return currentItemCount;
    }
  }

  public Heap(int maxHeapSize) {
    items = new T[maxHeapSize];
  }

  public void Add(T item) {
    item.HeapIndex = currentItemCount;
    items[currentItemCount] = item;
    SortUp(item);
    currentItemCount++;
  }

  public T RemoveFirst() {
    T first = items[0];
    currentItemCount--;
    items[0] = items[currentItemCount];
    items[0].HeapIndex = 0;
    SortDown(items[0]);
    return first;
  }

  public bool Contains(T item) {
    return Equals(items[item.HeapIndex], item);
  }

  public void UpdateItem(T item) {
    SortUp(item);
  }

  void SortUp(T item) {
    int parentIndex = (item.HeapIndex - 1) / 2;

    while (true) {
      T parentItem = items[parentIndex];
      if (item.CompareTo(parentItem) > 0) {
        Swap(item, parentItem);
      }
      else {
        break;
      }
      parentIndex = (item.HeapIndex - 1) / 2;
    }
  }

  void Swap(T item1, T item2) {
    items[item1.HeapIndex] = item2;
    items[item2.HeapIndex] = item1;
    int temp = item1.HeapIndex;
    item1.HeapIndex = item2.HeapIndex;
    item2.HeapIndex = temp;
  }

  void SortDown(T item) {
    while (true) {
      int leftChildIndex = item.HeapIndex * 2 + 1;
      int rightChildIndex = item.HeapIndex * 2 + 2;
      int swapIndex = 0;

      if (leftChildIndex < currentItemCount) {
        swapIndex = leftChildIndex;

        if (rightChildIndex < currentItemCount) {
          if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0) {
            swapIndex = rightChildIndex;
          }
        }

        if (item.CompareTo(items[swapIndex]) < 0) {
          Swap(item, items[swapIndex]);
        }
        else {
          return;
        }
      }
      else {
        return;
      }
    }
  }

}

public interface IHeapItem<T> : IComparable<T> {
  int HeapIndex {
    get;
    set;
  }
}
