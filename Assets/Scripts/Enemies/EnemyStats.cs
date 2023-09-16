/*******************************************************************************
* File Name :         EnemyStats.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/5/2023
*
* Brief Description : Base enemy stats. Applies to all enemies
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]

public class EnemyStats : ScriptableObject
{
    public float Speed;
    [Tooltip("Make true if enemy moves towards player")]
    public bool EnemyMove;
    [Tooltip("Leave as 0 for continous movement. After an enemy moves, it should stop briefly, before moving again")]
    public float TimeBetweenMovements;

    public int HealthPoints;

    [Tooltip("Amount of damage the enemies attack deals")]
    public int Damage;
    public int ContactDamage;

    public float KnockBackForce;

    public bool DealContactDamage;
}
