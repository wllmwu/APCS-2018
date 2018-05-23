﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	public Transform target;
  float speed = 0.3f;
  Vector3[] path;
  int pathTargetIndex;
  public bool shouldDrawGizmos;

  Vector3 lookDirection;

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
      if (transform.position == nextWaypoint) {
        pathTargetIndex++;
        if (pathTargetIndex >= path.Length) {
          yield break;
        }
        nextWaypoint = path[pathTargetIndex];
      }
      transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed);
      Quaternion look;
      if (TargetInLineOfSight()) {
        look = Quaternion.LookRotation(target.position - transform.position);
      }
      else {
        look = Quaternion.LookRotation(nextWaypoint - transform.position);
      }
      transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5);
      yield return null;
    }
  }

  bool TargetInLineOfSight() {
    Debug.Log("transform: " + transform.position + " target: " + target.position);
    lookDirection = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
    Debug.Log("look direction: " + lookDirection);
    RaycastHit hit;
    //if (Physics.Raycast(transform.position, lookDirection, out hit)) {
    if (Physics.Raycast(transform.position + Vector3.up, target.position - transform.position, out hit)) {
      if (hit.transform == target) {
        Debug.Log("in LOS");
        return true;
      }
      Debug.Log("raycast hit " + hit.transform.name);
    }
    Debug.Log("not in LOS");
    return false;
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
    Gizmos.DrawLine(transform.position + Vector3.up, transform.position + lookDirection + Vector3.up);
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
