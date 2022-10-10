using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public Slider Healthslider;
    public Animator animator;
    public EnemyController ec;
    public UnityEngine.AI.NavMeshAgent nma;
    private Rigidbody rigidbody;
    public float speed = 50f;
    
    void Start()
    {
        health = 100f;
    }

    void FixedUpdate()
    {
        if (health <= 0f)
        {
            animator.SetBool("SeePlayer", false);
            Invoke("Die", 2);
            ec.enabled = false;
            gameObject.AddComponent<Rigidbody>();
            rigidbody = gameObject.GetComponent<Rigidbody>();
            nma.enabled = false;
            rigidbody.AddForce(transform.forward * speed);
        }

        Healthslider.value = health;
    }

    void Die ()
    {
        Destroy(gameObject);
    }
}
