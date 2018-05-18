
using UnityEngine;

public class Gun : MonoBehaviour {
	public float damage = 10f;
	public float fireRate = 4f;
	public float nextTimeToFire = 0f;

	public Camera fpsCam;

	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
			Shoot();
			nextTimeToFire = Time.time + 4f/fireRate;
		}
	}

	void Shoot(){
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit)){
			Debug.Log(hit.transform.name);
		}

		Entity entity = hit.transform.GetComponent<Entity>();
		if (entity != null){
			entity.TakeDamage(damage);
		}
	}
}
