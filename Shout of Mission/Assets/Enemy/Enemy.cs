using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	public Transform target;
  float speed = 0.2f;
  Vector3[] path;
  int pathTargetIndex;
  Vector3 lookDirection;
  public bool shouldDrawGizmos;
  float minDistanceToTarget = 20f;
  public float damage = 4f;
  public float spread = 0.05f;

  public GameObject barrelEnd;
	public ParticleSystem muzzleFlash;
	public AudioSource shootSound;
	public WFX_LightFlicker light;
	public GameObject impactEffect;

  public bool isOriginal;
  Player player;

  void Start() {
    if (!isOriginal) {
      player = GameObject.Find("FPSController").GetComponent<Player>();
      Invoke("RequestNewPath", 0.1f);
    }
  }

  void RequestNewPath() {
    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
  }

  public void OnPathFound(Vector3[] newPath, bool success) {
    if (success) {
      gameObject.SetActive(true);
      path = newPath;
      pathTargetIndex = 0;
      StopCoroutine("FollowPath");
      StopCoroutine("FireAtTarget");
      if (path.Length > 0) {
        StartCoroutine("FollowPath");
      }
      else {
        StartCoroutine("FireAtTarget");
      }
    }
    else {
      Debug.Log("no path");
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
          //Debug.Log("end of path at index " + pathTargetIndex);
          Invoke("RequestNewPath", 0.1f);
          yield break;
        }
        nextWaypoint = path[pathTargetIndex];
      }
      if (targetInLOS && DistanceToTarget() <= minDistanceToTarget) {
        StartCoroutine("FireAtTarget");
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
    //lookDirection = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
    lookDirection = target.position - transform.position - Vector3.up * 1.1f;
    RaycastHit hit;
    if (Physics.Raycast(transform.position + Vector3.up, lookDirection, out hit) && hit.transform == target) {
      return true;
    }
    //Debug.Log("not in los -- target " + target.position + " -- transform " + transform.position);
    return false;
  }

  float DistanceToTarget() {
    float dist = Vector3.Distance(target.position - Vector3.up * target.position.y, transform.position - Vector3.up * transform.position.y);
    //Debug.Log("distance " + dist);
    return dist;
  }

  IEnumerator FireAtTarget() {
    while (true) {
      if (TargetInLineOfSight() && DistanceToTarget() <= minDistanceToTarget) {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 20);
        Fire();
        yield return new WaitForSeconds(0.1f);
      }
      else {
        RequestNewPath();
        //Debug.Log("new path requested");
        yield break;
      }
    }
  }

  void Fire() {
    muzzleFlash.Play();
		shootSound.Play();
		light.beginFlash();
		RaycastHit hit;

    float xChange = Random.Range(-spread, spread);
    float yChange = Random.Range(-spread, spread);
    float zChange = Random.Range(-spread, spread);
    Vector3 fireDirection = transform.forward + Vector3.right * xChange + Vector3.up * yChange + Vector3.forward * zChange;
		if (Physics.Raycast(transform.position + Vector3.up * 1.5f, fireDirection, out hit)){
			Debug.Log(hit.transform.name);
		}

		Entity entity = hit.transform.GetComponent<Entity>();
		if (entity != null){
			entity.TakeDamage(damage);
      Debug.Log(entity.health);
		}
		
		GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
		Destroy(impact, 1f);
  }

  public override void TakeDamage(float amount) {
    base.TakeDamage(amount);
    if (health <= 0) {
      player.IncrementScore();
    }
  }

  public override void Die() {
    StopAllCoroutines();
    gameObject.SetActive(false);
    Invoke("Respawn", 1);
  }

  void Respawn() {
    LevelGenerator.PlaceEnemy(gameObject);
    health = 100f;
    RequestNewPath();
  }

  public void Stop() {
    enabled = false;
    StopAllCoroutines();
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
