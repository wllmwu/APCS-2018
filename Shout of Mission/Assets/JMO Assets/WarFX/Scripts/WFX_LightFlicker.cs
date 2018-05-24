using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public float time = 0.05f;
	private bool flash;
	private float timer;
	
	void Start ()
	{
		timer = time;
		flash = false;
		// StartCoroutine("Flicker");
	}
	
	void Update(){
		if (flash){
			GetComponent<Light>().enabled = true;
			flash = false;
		}
		else{
			GetComponent<Light>().enabled = false;
		}
	}
	IEnumerator Flicker()
	{
		while(true)
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			
			do
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			timer = time;
		}
	}

	public void beginFlash(){
		flash = true;
	}
}
