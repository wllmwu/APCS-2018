
using UnityEngine;

public class Rocket : MonoBehaviour {
	
	public GameObject explode;
	// public ParticleSystem explode2;
	
	// public MeshRenderer renderer;
	void OnCollisionEnter(Collision col){
		// if (col.collider.name!= "BarrelEnd" && col.collider.name != "SMAW"){
			// renderer.enabled = false;
			ContactPoint contact = col.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        	Vector3 pos = contact.point;
			GameObject explosion = Instantiate(explode, pos, rot);
			Destroy(gameObject);
		// }

	}
}
