/*******************************************************************************
* File Name :         PlayerBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : The player class which deals specifically with everything 
* but player controls.
* Meant to avoid clutter
* 
* TODO:
* Collisions
* Health
* Upgrades???
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //Player Stats
    public PlayerStats CurrentPlayerStats;
    [Header("Derived from PlayerStats, do not tweak in editor")]
    public float Speed;
    public int HealthPoints;
    public float DashRechargeSeconds;

    public float PrimaryAttackDamage;
    public float PrimaryAttackSpeed;
    public float SecondaryAttackDamage;
    public float SecondaryAttackSpeed;

    //components
    DefaultPlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<DefaultPlayerController>();
    }

    public void SetStatsToDefaults()
    {
        Speed = CurrentPlayerStats.Speed;
        HealthPoints = CurrentPlayerStats.HealthPoints;
        DashRechargeSeconds = CurrentPlayerStats.DashRechargeSeconds;
        
        PrimaryAttackDamage = CurrentPlayerStats.PrimaryAttackDamage;
        PrimaryAttackSpeed = CurrentPlayerStats.PrimaryAttackSpeed;
        SecondaryAttackDamage = CurrentPlayerStats.SecondaryAttackSpeed;
        SecondaryAttackSpeed = CurrentPlayerStats.SecondaryAttackDamage;
    }
}
