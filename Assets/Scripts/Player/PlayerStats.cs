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

    public int MaxHealthPoints;

    public float InvincibilitySeconds;

    [Header("Dash")]
    [Tooltip("Seconds the player takes to dash")]
    public float DashTime;
    [Tooltip("How many units the player will dash")]
    public float DashUnits;
    public float DashRechargeSeconds;

    [Header("Primary Attack")]
    public int PrimaryAttackDamage;
    [Tooltip("Seconds for Primary attack to complete")]
    public float PrimaryAttackSpeed;
    public float PrimaryAttackCoolDown;
    public float PrimaryAttackKnockback;

    [Header("Secondary Attack")]
    public int SecondaryAttackDamage;
    [Tooltip("Seconds for Secondary attack to complete")]
    public float SecondaryAttackSpeed;
    public float SecondaryAttackCoolDown;
    public float SecondaryAttackKnockback;
}
