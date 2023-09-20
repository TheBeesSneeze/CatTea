/*******************************************************************************
* File Name :         PlayerHealthBar.cs
* Author(s) :         Alex Bell
* Creation Date :     9/18/2023
*
* Brief Description : The Player's health bar.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private int MaxHealth;
    private PlayerBehaviour playerBehaviour;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        MaxHealth = playerBehaviour.HealthPoints;
        healthBar.maxValue = MaxHealth;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        healthBar.value = playerBehaviour.HealthPoints;
    }
}
