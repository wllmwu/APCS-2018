
using UnityEngine;

public class Gun : MonoBehaviour {
	public float damage = 10f;
	public float fireRate = 4f;
	public float nextTimeToFire = 0f;
  public float spread = 0.03f;
  public int clipSize;
  public int clipBullets;
  public int remainingBullets;

	public Camera fpsCam;
	public ParticleSystem muzzleFlash;
	public AudioSource shootSound;
	public WFX_LightFlicker light;
	public GameObject impactEffect;
	private Animation shootAnimation;
	public bool autoFire;
  bool reloading = false;

  Hud hud;

	void Start(){
		shootAnimation =  GetComponent<Animation>();
    hud = GameObject.Find("Canvas").GetComponent<Hud>();
    hud.UpdateAmmoText(clipBullets, remainingBullets);
	}
	// Update is called once per frame
	void Update () {
    if (clipBullets > 0 && !reloading) {
      if (autoFire && Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
        Shoot();
        nextTimeToFire = Time.time + 1f/fireRate;
      }
      else if (!autoFire && Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire){
        Shoot();
        nextTimeToFire = Time.time + 1f/fireRate;
      }
    }
    if (!reloading && clipBullets < clipSize && Input.GetButtonDown("Fire2")) {
      reloading = true;
      hud.SetReloading(true);
      Invoke("Reload", 2);
    }
	}

	void Shoot(){
		muzzleFlash.Play();
		shootSound.Play();
		light.beginFlash();
		shootAnimation.Play();
		RaycastHit hit;

    float xChange = Random.Range(-spread, spread);
    float yChange = Random.Range(-spread, spread);
    float zChange = Random.Range(-spread, spread);
    Vector3 fireDirection = fpsCam.transform.forward + Vector3.right * xChange + Vector3.up * yChange + Vector3.forward * zChange;
		if (Physics.Raycast(fpsCam.transform.position, fireDirection, out hit)){
			Debug.Log(hit.transform.name);
		}

		Entity entity = hit.transform.GetComponent<Entity>();
		if (entity != null){
			entity.TakeDamage(damage);
		}
		
		GameObject temp = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
		Destroy(temp, 1f);

    clipBullets--;
    hud.UpdateAmmoText(clipBullets, remainingBullets);
    if (clipBullets <= 0) {
      hud.SetReloading(true);
      Invoke("Reload", 2);
    }
	}

  void Reload() {
    while (clipBullets < clipSize && remainingBullets > 0) {
      clipBullets++;
      remainingBullets--;
    }
    hud.UpdateAmmoText(clipBullets, remainingBullets);
    hud.SetReloading(false);
    reloading = false;
  }
}
