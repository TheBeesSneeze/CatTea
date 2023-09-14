/*******************************************************************************
* File Name :         EnemyBehaviour.cs
* Author(s) :         Toby Schamberger, Elda Osami
* Creation Date :     9/13/2023
*
* Brief Description : inherits cool stats from EnemyStats. Uses awesome pathfinding
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    public EnemyStats CurrentEnemyStats;

    [Header("Don't touch these in editor")]
    public int Damage;
    public bool EnemyMove; // if enemy should be stationary or nah
    public float TimeBetweenMovements;
    protected int contactDamage;
    protected bool dealContactDamage;

    protected override void Start()
    {
        base.Start();

        if(EnemyMove) 
        {
            StartMovingToPlayer();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    /// <summary>
    /// Hurts the player if they touch them
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            Debug.Log("hi");
            PlayerBehaviour playerBehavior = collision.gameObject.GetComponent<PlayerBehaviour>();

            playerBehavior.TakeDamage(contactDamage, this.transform.position, KnockbackForce);
        }
    }

    public void StartMovingToPlayer()
    {
        // @TODO (elda)
        // you might need to call a coroutine?? idk man. good luck tho (lmk if you need help ofc)
        // check the TDD for full rundown of the logic PLEASE
    }

    public override void SetStatsToDefaults()
    {
        Speed = CurrentEnemyStats.Speed;
        EnemyMove = CurrentEnemyStats.EnemyMove;
        TimeBetweenMovements = CurrentEnemyStats.TimeBetweenMovements;

        HealthPoints = CurrentEnemyStats.HealthPoints;
        Damage = CurrentEnemyStats.Damage;
        contactDamage = CurrentEnemyStats.ContactDamage;
        dealContactDamage = CurrentEnemyStats.DealContactDamage;    

        KnockbackForce = CurrentEnemyStats.KnockBackForce;
    }
}
