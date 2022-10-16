using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject Bullet;
    public float fireRate, nextFire;
    public float ShootForce;
    public Transform GunTip;
    public ParticleSystem MuzzleFlash;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(Time.time >= nextFire)
            {
                nextFire = Time.time +1f / fireRate;
                ShootBullet();
            }
        }
    }

    void ShootBullet ()
    {
        MuzzleFlash.Play();
        GameObject clone = Instantiate(Bullet,GunTip.position,transform.rotation * Quaternion.Euler (90, 90, 90));
        clone.GetComponent<Rigidbody>().AddForce(transform.forward * ShootForce);
        Destroy(clone, 10);
    }
}
