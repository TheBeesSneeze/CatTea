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
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {        
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        MaxHealth = PlayerBehaviour.Instance.MaxHealthPoints;
        healthBar.maxValue = MaxHealth;
        healthBar.value = PlayerBehaviour.Instance.HealthPoints;
    }
}
