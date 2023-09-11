using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]

public class EnemyStats : ScriptableObject
{
    public float Speed;

    public int HealthPoints;

    [Tooltip("Amount of damage the enemies attack deals")]
    public int Damage;
    public int ContactDamage;

    public float KnockBackForce;

    public bool DealContactDamage;
}
