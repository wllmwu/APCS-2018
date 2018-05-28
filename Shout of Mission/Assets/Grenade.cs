using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    float time;
    Rigidbody grenade;

    bool inAir;
    bool exploded;
    float radius;
    Vector3 explodeLoc;
    public float damage;
    public GameObject impactEffect;
    AudioSource explodeSound;

    // Use this for initialization
    void Start()
    {
        grenade = GetComponent<Rigidbody>();
        time = 2;
        inAir = false;
        radius = 3;
        damage = 100;
        explodeSound = GetComponent<AudioSource>();

        exploded = false;


    }

    // Update is called once per frame
    void Update()
    {
        // print("Pressed");
        if (Input.GetButtonDown("Fire1"))
        {
            throwing();
            inAir = true;
        }
        if (inAir == true)
        {
            time -= Time.deltaTime;
        }
        if (time < 0f && exploded == false)
        {

            explode();

        }
    }

    void throwing()
    {
        transform.parent = null;
        grenade.AddForce(grenade.transform.forward * 500);
        grenade.useGravity = true;
        grenade.constraints = RigidbodyConstraints.None;

        // grenade.constraints = RigidbodyConstraints.FreezePositionX;
        // grenade.constraints = RigidbodyConstraints.FreezePositionY;
        // grenade.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    void explode()
    {
        explodeLoc = transform.position;

        explodeSound.Play();




        Collider[] hitColliders = Physics.OverlapSphere(explodeLoc, radius);
        foreach (Collider c in hitColliders)
        {
            Entity entity = c.transform.GetComponent<Entity>();
            if (entity != null)
            {
                entity.TakeDamage(damage);

            }
        }
        // Destroy(gameObject);
        GameObject temp = Instantiate(impactEffect, explodeLoc, Quaternion.LookRotation(explodeLoc));
        transform.Translate(0, 1000, 0);


        Destroy(temp, 1f);
        Destroy(gameObject, 1f);
        exploded = true;

    }
}
