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
    //obviously most of these stats wont apply to the default player

    [Header("Don't change any of these in code.")]
    public float Speed;

    public int HealthPoints;

    public float DashRechargeSeconds;

    public float PrimaryAttackDamage;
    [Tooltip("Seconds for Primary attack to complete")]
    public float PrimaryAttackSpeed;

    public float SecondaryAttackDamage;
    [Tooltip("Seconds for Secondary attack to complete")]
    public float SecondaryAttackSpeed;
}
