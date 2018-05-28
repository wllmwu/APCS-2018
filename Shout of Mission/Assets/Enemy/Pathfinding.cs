using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour {

  NodeGrid grid;
  PathRequestManager requestManager;

  void Awake() {
    grid = GetComponent<NodeGrid>();
    requestManager = GetComponent<PathRequestManager>();
  }

  public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
    StartCoroutine(FindPath(startPos, targetPos));
  }

  IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition) {
    Vector3[] waypoints = new Vector3[0];
    bool success = false;

    Node startNode = grid.NodeFromWorldPoint(startPosition);
    Node targetNode = grid.NodeFromWorldPoint(targetPosition);
    Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
    HashSet<Node> closedSet = new HashSet<Node>();
    openSet.Add(startNode);
    while (openSet.Count > 0) {
      Node currentNode = openSet.RemoveFirst();
      closedSet.Add(currentNode);

      if (currentNode == targetNode) {
        success = true;
        break;
      }

      foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
        if (!neighbor.walkable && neighbor != targetNode || closedSet.Contains(neighbor)) {
          continue;
        }

        int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
        if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
          neighbor.gCost = newMovementCostToNeighbor;
          neighbor.hCost = GetDistance(neighbor, targetNode);
          neighbor.parent = currentNode;

          if (!openSet.Contains(neighbor)) {
            openSet.Add(neighbor);
          }
          else {
            openSet.UpdateItem(neighbor);
          }
        }
      }
    }
    yield return null;
    if (success) {
      waypoints = TracePath(startNode, targetNode);
    }
    requestManager.FinishProcessing(waypoints, success);
  }

  Vector3[] TracePath(Node start, Node end) {
    List<Node> path = new List<Node>();
    Node current = end;
    if (start == end) {
      path.Add(start);
    }
    else {
      while (current != start) {
        path.Add(current);
        current = current.parent;
      }
    }
    Vector3[] waypoints = SimplifyPath(path);
    Array.Reverse(waypoints);
    return waypoints;
  }

  Vector3[] SimplifyPath(List<Node> path) {
    List<Vector3> waypoints = new List<Vector3>();
    for (int i = 1; i < path.Count - 1; i++) {
      Vector2 newDirectionBack = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
      Vector2 newDirectionForward = new Vector2(path[i].gridX - path[i + 1].gridX, path[i].gridY - path[i + 1].gridY);
      if (newDirectionBack != newDirectionForward) {
        waypoints.Add(path[i].worldPosition - Vector3.up * 3);
      }
    }
    return waypoints.ToArray();
  }

  int GetDistance(Node nodeA, Node nodeB) {
    int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
    int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
    if (distX > distY) {
      return 14 * distY + 10 * (distX - distY);
    }
    return 14 * distX + 10 * (distY - distX);
  }

}
