using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

  public Transform seeker, target;
	NodeGrid grid;

  void Awake() {
    grid = GetComponent<NodeGrid>();
  }

  void Update() {
    FindPath(seeker.position, target.position);
  }

  void FindPath(Vector3 startPosition, Vector3 targetPosition) {
    Node startNode = grid.NodeFromWorldPoint(startPosition);
    Node targetNode = grid.NodeFromWorldPoint(targetPosition);

    List<Node> openSet = new List<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();

    openSet.Add(startNode);
    while (openSet.Count > 0) {
      Node currentNode = openSet[0];
      for (int i = 1; i < openSet.Count; i++) {
        if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
          currentNode = openSet[i];
        }
      }

      openSet.Remove(currentNode);
      closedSet.Add(currentNode);

      if (currentNode == targetNode) {
        TracePath(startNode, targetNode);
        return;
      }

      foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
        if (!neighbor.walkable || closedSet.Contains(neighbor)) {
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
        }
      }
    }
  }

  void TracePath(Node start, Node end) {
    List<Node> path = new List<Node>();
    Node current = end;
    while (current != start) {
      path.Add(current);
      current = current.parent;
    }
    path.Reverse();
    grid.path = path;
  }

  int GetDistance(Node nodeA, Node nodeB) {
    int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
    int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
    if (distX < distY) {
      return 14 * distY + 10 * (distX - distY);
    }
    return 14 * distX + 10 * (distY - distX);
  }

}
