using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddForce : MonoBehaviour
{
    public Rigidbody rigidBody;
    public Camera cameraGameObject;
    public float speed = 1000f;
    public float cooldownTime = 2;
    public float nextFireTime = 0;
    public CameraShake cameraShake;
    public Slider DashSlider;
    
    void Update()
    {
        if (Time.time > nextFireTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<AudioManager>().Play("Dash");
                rigidBody.AddForce(cameraGameObject.transform.forward * speed);
                StartCoroutine(cameraShake.Shake(.15f, .4f));
                nextFireTime = Time.time + cooldownTime;
            }
        }

        speed = DashSlider.value;
    }
}