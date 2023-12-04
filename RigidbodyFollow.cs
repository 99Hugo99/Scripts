using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public Transform FollowObject;
    Rigidbody rigidbody;
    public float MaxSpeed;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbody.velocity = (FollowObject.position - transform.position) / Time.fixedDeltaTime;
        rigidbody.MoveRotation(FollowObject.rotation);
        if(rigidbody.velocity.magnitude > MaxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
        }
    }
}
