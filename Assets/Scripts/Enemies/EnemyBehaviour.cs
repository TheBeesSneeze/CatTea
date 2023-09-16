/*******************************************************************************
* File Name :         EnemyBehaviour.cs
* Author(s) :         Toby Schamberger, Elda Osami
* Creation Date :     9/13/2023
*
* Brief Description : inherits cool stats from EnemyStats. Uses awesome pathfinding
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    [Header("Eenemy Stat Sheet:")]
    public EnemyStats CurrentEnemyStats;

    [Header("Don't touch these in editor")]
    public int Damage;
    public bool EnemyMove; // if enemy should be stationary or nah
    public float TimeBetweenMovements;
    protected int contactDamage;
    protected bool dealContactDamage;

    private PlayerBehaviour playerBehavior;

    protected override void Start()
    {
        base.Start();

        if(EnemyMove) 
        {
            StartMovingToPlayer();
        }

        playerBehavior = GameObject.FindObjectOfType<PlayerBehaviour>();
    }

    /// <summary>
    /// runs when player collides with enemy.
    /// </summary>
    protected virtual void OnPlayerCollision(Collider2D collision)
    {
        Debug.Log("player on enemy contact???");

        playerBehavior.TakeDamage(contactDamage, this.transform.position, KnockbackForce);
    }

    protected virtual void OnPrimaryAttackCollision(Collider2D collision)
    {

    }

    protected virtual void OnSecondaryAttackCollision(Collider2D collision)
    {

    }

    protected virtual void StartMovingToPlayer()
    {
        // @TODO (elda)
        // you might need to call a coroutine?? idk man. good luck tho (lmk if you need help ofc)
        // check the TDD for full rundown of the logic PLEASE
    }

    /// <summary>
    /// Calls various collision functions
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
            OnPlayerCollision(collision);

        if (tag.Equals("Primary Player Attack"))
            OnPrimaryAttackCollision(collision);

        if (tag.Equals("Secondary Player Attack"))
            OnPrimaryAttackCollision(collision);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
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
