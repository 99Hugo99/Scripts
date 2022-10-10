using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerDie playerDie;
    public float playerHealth;
    public Slider PlayerHealthBar;

    void Start()
    {
        playerHealth = 100f;
    }

    void FixedUpdate()
    {
        if (playerHealth <= 0f)
        {
            playerDie.Die();
        }

        PlayerHealthBar.value = playerHealth;
    }
}
