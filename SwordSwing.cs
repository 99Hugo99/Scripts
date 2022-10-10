using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public Animator animator;
    private float nextTimeToFire = 0f;
    public float fireRate = 1f;
    public GameObject trail;
    private bool Swinging;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            trail.SetActive(true);
            animator.SetBool("Swinging", true);
            Swinging = true;
        }
    }

    public void StopSwing ()
    {
        animator.SetBool("Swinging", false);
        trail.SetActive(false);
        Swinging = false;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Enemy" && Swinging == true)
        {
            other.gameObject.GetComponent<EnemyHealth>().health -= 50f;
        }
    }
}