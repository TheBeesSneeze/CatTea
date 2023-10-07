/*******************************************************************************
* File Name :         PlayerAmmoBar.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/6/2023
*
* Brief Description : The Player's ammo bar.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoBar : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;
    private RangedPlayerController rangedPlayer;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        rangedPlayer = GameObject.FindObjectOfType<RangedPlayerController>();

        UpdateAmmo();
    }

    public void UpdateAmmo()
    {
        if (playerBehaviour == null)
            return;

        healthBar.maxValue = playerBehaviour.ShotsPerBurst;
        healthBar.value = rangedPlayer.BulletsLeft;
    }
}
