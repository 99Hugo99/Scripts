using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public GameObject Arrow;
    public float fireRate, nextFire;
    public float ShootForce;
    public Transform Bow;
    public ParticleSystem Effect;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(Time.time >= nextFire)
            {
                nextFire = Time.time +1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot ()
    {
        FindObjectOfType<AudioManager>().Play("ShootArrow");
        Effect.Play();
        GameObject clone = Instantiate(Arrow,Bow.position,transform.rotation * Quaternion.Euler (90, 90, 90));
        clone.GetComponent<Rigidbody>().AddForce(transform.forward * ShootForce);
        Destroy(clone, 10);
    }
}
