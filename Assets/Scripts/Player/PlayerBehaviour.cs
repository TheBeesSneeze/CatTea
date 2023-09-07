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
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : CharacterBehaviour
{
    //Player Stats
    public PlayerStats CurrentPlayerStats;
    [Header("Derived from PlayerStats, do not tweak in editor")]
    public float DashRechargeSeconds;
    public float DashForce;

    public float PrimaryAttackDamage;
    public float PrimaryAttackSpeed;
    public float SecondaryAttackDamage;
    public float SecondaryAttackSpeed;

    //components
    private DefaultPlayerController playerController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<DefaultPlayerController>();
        SetStatsToDefaults();
    }

    public override void SetStatsToDefaults()
    {
        Speed = CurrentPlayerStats.Speed;
        HealthPoints = CurrentPlayerStats.HealthPoints;
        DashRechargeSeconds = CurrentPlayerStats.DashRechargeSeconds;
        DashForce = CurrentPlayerStats.DashForce;
        
        PrimaryAttackDamage = CurrentPlayerStats.PrimaryAttackDamage;
        PrimaryAttackSpeed = CurrentPlayerStats.PrimaryAttackSpeed;
        SecondaryAttackDamage = CurrentPlayerStats.SecondaryAttackSpeed;
        SecondaryAttackSpeed = CurrentPlayerStats.SecondaryAttackDamage;
    }
}
