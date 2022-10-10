using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviour
{
    public GameObject DeathCam, MainCam, Orientation, DeathScreen, GameHUD;
    public Rigidbody rigidbody;
    public MapOpen mapOpen;
    public AddForce af;
    public PlayerMovement pm;

    public void Die ()
    {
        DeathCam.SetActive(true);
        MainCam.SetActive(false);
        Orientation.SetActive(false);
        DeathScreen.SetActive(true);
        GameHUD.SetActive(false);
        rigidbody.constraints = RigidbodyConstraints.None;
        mapOpen.enabled = false;
        af.enabled = false;
        pm.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Retry ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
