using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Childcount : MonoBehaviour
{
    void FixedUpdate()
    {
        if (transform.childCount <= 0f)
        {
            Invoke("NextScene", 2f);
        }
    }

    void NextScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
