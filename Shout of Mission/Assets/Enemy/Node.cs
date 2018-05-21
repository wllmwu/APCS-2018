using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

	public bool walkable;
  public Vector3 worldPosition;
  public int gridX, gridY;

  public Node parent;
  public int gCost; // distance from start
  public int hCost; // distance to target
  public int fCost {
    get {
      return gCost + hCost;
    }
  }
  int heapIndex;

  public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY) {
    walkable = _walkable;
    worldPosition = _worldPosition;
    gridX = _gridX;
    gridY = _gridY;
  }

  public int HeapIndex {
    get {
      return heapIndex;
    }
    set {
      heapIndex = value;
    }
  }

  public int CompareTo(Node other) {
    int compare = fCost.CompareTo(other.fCost);
    if (compare == 0) {
      compare = hCost.CompareTo(other.hCost);
    }
    return -compare;
  }

}
