using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	public Transform target;
  float speed = 0.3f;
  Vector3[] path;
  int pathTargetIndex;
  public bool shouldDrawGizmos;

  void Start() {
    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
  }

  public void OnPathFound(Vector3[] newPath, bool success) {
    if (success) {
      path = newPath;
      StopCoroutine("FollowPath");
      StartCoroutine("FollowPath");
    }
    else {
      Die();
    }
  }

  IEnumerator FollowPath() {
    Vector3 nextWaypoint = path[0];
    while (true) {
      if (transform.position == nextWaypoint) {
        pathTargetIndex++;
        if (pathTargetIndex >= path.Length) {
          yield break;
        }
        nextWaypoint = path[pathTargetIndex];
      }
      transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed);
      Quaternion look = Quaternion.LookRotation(nextWaypoint - transform.position);
      transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5);
      yield return null;
    }
  }

  void Fire() {
    //
  }

  public void OnDrawGizmos() {
    if (shouldDrawGizmos) {
      if (path != null) {
        Gizmos.color = Color.black;
        for (int i = 1; i < path.Length; i++) {
          Gizmos.DrawCube(path[i], Vector3.one);
          if (i == pathTargetIndex) {
            Gizmos.DrawLine(transform.position, path[i]);
          }
          else {
            Gizmos.DrawLine(path[i - 1], path[i]);
          }
        }
      }
    }
  }

}
