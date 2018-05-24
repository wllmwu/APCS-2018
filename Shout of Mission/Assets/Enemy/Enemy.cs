using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	public Transform target;
  float speed = 0.3f;
  Vector3[] path;
  int pathTargetIndex;
  Vector3 lookDirection;
  public bool shouldDrawGizmos;

  void Start() {
    Invoke("RequestNewPath", 0.1f);
  }

  void RequestNewPath() {
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
      bool targetInLOS = TargetInLineOfSight();
      if (transform.position == nextWaypoint) {
        pathTargetIndex++;
        if (pathTargetIndex >= path.Length) {
          yield break;
        }
        nextWaypoint = path[pathTargetIndex];
      }
      if (targetInLOS && DistanceToTarget() <= 20) {
        yield break;
      }
      transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed);
      Quaternion look;
      if (targetInLOS) {
        look = Quaternion.LookRotation(lookDirection);
      }
      else {
        look = Quaternion.LookRotation(nextWaypoint - transform.position);
      }
      transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5);
      yield return null;
    }
  }

  bool TargetInLineOfSight() {
    lookDirection = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
    RaycastHit hit;
    if (Physics.Raycast(transform.position + Vector3.up, lookDirection, out hit) && hit.transform == target) {
      return true;
    }
    return false;
  }

  float DistanceToTarget() {
    float dist = Vector3.Distance(target.position, transform.position);
    Debug.Log("distance: " + dist);
    return dist;
  }

  void Fire() {
    //muzzleFlash.Play();
    //shootSound.Play();
    /*RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit)){
			Debug.Log(hit.transform.name);
		}

		Entity entity = hit.transform.GetComponent<Entity>();
		if (entity != null){
			entity.TakeDamage(damage);
		}*/
  }

  public void OnDrawGizmos() {
    Gizmos.color = Color.blue;
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
