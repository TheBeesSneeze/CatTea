/*******************************************************************************
* File Name :         PlayerHealthBar.cs
* Author(s) :         Alex Bell, Toby Schamberger
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
    private float MaxHealth;
    private PlayerBehaviour playerBehaviour;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if(playerBehaviour == null)
            return;
            
        MaxHealth = playerBehaviour.MaxHealthPoints;
        healthBar.maxValue = MaxHealth;
        healthBar.value = playerBehaviour.HealthPoints;
    }
}
