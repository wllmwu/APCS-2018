﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

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

  public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY) {
    walkable = _walkable;
    worldPosition = _worldPosition;
    gridX = _gridX;
    gridY = _gridY;
  }

}
