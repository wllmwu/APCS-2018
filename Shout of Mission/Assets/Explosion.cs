
using UnityEngine;

public class Explosion : MonoBehaviour {
	public float radius = 5;
	public float damage;
	public AudioSource explodeSound;
	// Use this for initialization
	void Start () {
		Debug.Log("explode!!!");
		Vector3 explodePos = gameObject.transform.position;
		Collider[] hitColliders = Physics.OverlapSphere(explodePos, radius);
		foreach (Collider c in hitColliders)
        {
            Entity entity = c.transform.GetComponent<Entity>();
            if (entity != null)
            {
                entity.TakeDamage(damage);
				if (entity.GetComponent<Rigidbody>() != null){
				entity.GetComponent<Rigidbody>().AddExplosionForce(6000, explodePos, radius);
			}

            }

        }
		Destroy(gameObject, 5.5f);

	}

	// void OnCollisionEnter(Collision col){
	// 	Enemy enemy = col.gameObject.GetComponent<Enemy>();
	// 	if (col.collider.GetComponent<Entity>() != null)
	// }

}
