using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{

    bool slashed;
    public Camera fpsCam;
    public float damage;
    public float range;
    public GameObject impactEffect;
    AudioSource hitSound;

    // Use this for initialization
    void Start()
    {
        slashed = false;

        hitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && slashed == false)
        {
            transform.Rotate(0f, 0f, -70f);
            slashed = true;
            slash();
        }
        if (Input.GetButtonUp("Fire1") && slashed == true)
        {
            transform.Rotate(0f, 0f, 70f);
            slashed = false;

        }

    }

    void slash()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }

        Entity entity = hit.transform.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(damage);
            hitSound.Play();
            GameObject temp = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(temp, 1f);
        }
    }
}