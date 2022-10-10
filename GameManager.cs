using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool Paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            Paused = true;
        }
    }
}
