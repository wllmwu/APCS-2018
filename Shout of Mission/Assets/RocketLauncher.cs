
using UnityEngine;

public class RocketLauncher : MonoBehaviour {

	public float fireRate = 1f;
	public float nextTimeToFire = 0f;
	public float shootSpeed = 6000f;

	public int clipSize = 4;
  	public int clipRockets = 4;
  	public int remainingRockets = 20;

	public Camera fpsCam;
	public Rigidbody rocket;
	public Transform barrelEnd;
	public ParticleSystem muzzleFlash;
	public AudioSource shootSound;
	Hud hud;
	bool reloading = false;
	
	// Update is called once per frame
	void Start(){
		hud = GameObject.Find("Canvas").GetComponent<Hud>();
	}
	void Update () {
		 if (clipRockets > 0 && !reloading) {
			if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire){
				Shoot();
				nextTimeToFire = Time.time + 1f/fireRate;
			}
		 }
		if (!reloading && clipRockets < clipSize && Input.GetButtonDown("Fire2") && remainingRockets > 0) {
      		reloading = true;
      		hud.SetReloading(true);
      		Invoke("Reload", 2);
   		 }
	}

	private void Shoot(){
		Rigidbody temp = Instantiate(rocket, barrelEnd.position, barrelEnd.rotation);
		temp.AddForce(barrelEnd.forward * shootSpeed);
		clipRockets--;
    	hud.UpdateAmmoText(clipRockets, remainingRockets);
   		if (clipRockets <= 0 && remainingRockets > 0) {
     		 hud.SetReloading(true);
      		Invoke("Reload", 2);
    	}
	}

	void Reload() {
    	while (clipRockets < clipSize && remainingRockets > 0) {
      clipRockets++;
      remainingRockets--;
      }
    	hud.UpdateAmmoText(clipRockets, remainingRockets);
   	 	hud.SetReloading(false);
   		reloading = false;
  }
}
