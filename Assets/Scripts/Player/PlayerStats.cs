/*******************************************************************************
* File Name :         PlayerStats.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : The basest base player stats. don't write over these
* values for they are sacred.
* Values are used in PlayerBehavior
* 
* TODO:
* impliment literally all of these. (these are all just numbers rn)
* add more as needed
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]

public class PlayerStats : ScriptableObject
{
    [Header("General")]
    //obviously most of these stats wont apply to the default player
    public float Speed;

    public float MaxHealthPoints;

    public float InvincibilitySeconds;

    [Header("Dash")]

    [Tooltip("Seconds the player takes to dash")]
    public float DashTime;
    [Tooltip("How many units the player will dash")]
    public float DashUnits;
    public float DashRechargeSeconds;

    [Header("Ranged Attack")]

    public int RangedAttackDamage;
    public float ProjectileSpeed;
    [Tooltip("Seconds for Primary attack to complete")]
    public int ShotsShotsPerBurst;
    [Tooltip("Seconds for Primary attack to complete")]
    public float TimeBetweenShots;
    public float AmmoRechargeTime;
    public float RangedAttackKnockback;

    [Header("Melee Attack")]
    public int MeleeAttackDamage;
    [Tooltip("Seconds for sword attack to complete")]
    public float SwingSeconds;
    public float SwordAttackCoolDown;
    public float MeleeAttackKnockback;
}
