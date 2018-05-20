using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

	NodeGrid grid;

  void Awake() {
    grid = GetComponent<NodeGrid>();
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
        return;
      }
    }
  }

}
