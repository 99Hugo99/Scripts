using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBack : MonoBehaviour
{
    public Rigidbody rigidBody;
    public Camera cameraGameObject;
    public float speed = 5000f;
    
    public void Hit ()
    {
        rigidBody.AddForce(cameraGameObject.transform.forward * -speed);
    }
}
