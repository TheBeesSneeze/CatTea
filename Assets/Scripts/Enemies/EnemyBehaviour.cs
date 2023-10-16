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
    public EnemyStats CurrentEnemyStats;

    [HideInInspector] public int DifficultyCost;
    [HideInInspector] public int Damage;
    [HideInInspector] public bool EnemyMove; // if enemy should be stationary or nah
    [HideInInspector] public float TimeBetweenMovements;
    [HideInInspector] protected int contactDamage;
    [HideInInspector] protected bool dealContactDamage;

    [HideInInspector] public EnemyRoom Room;

    //weird stuff
    private PlayerBehaviour playerBehavior;


    protected override void Start()
    {
        base.Start();
        transform.eulerAngles = Vector3.zero;

        if(EnemyMove) 
        {
            StartMovingToPlayer();
        }

        playerBehavior = GameObject.FindObjectOfType<PlayerBehaviour>();
    }

    /// <summary>
    /// Runs when enemy is spawned in.
    /// </summary>
    public virtual void OnSpawn()
    {
        //TODO
    }

    public override bool TakeDamage(int damage)
    {
        GameEvents.Instance.OnEnemyDamage(this.transform.position);

        return base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameEvents.Instance.OnEnemyDeath(this.transform.position);

        if(Room!= null) 
            Room.OnEnemyDeath();

        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// runs when player collides with enemy.
    /// </summary>
    protected virtual void OnPlayerCollision(Collider2D collision)
    {
        Debug.Log("player on enemy contact???");

        playerBehavior.TakeDamage(contactDamage, this.transform.position, 1);
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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    public override void SetStatsToDefaults()
    {
        DifficultyCost = CurrentEnemyStats.DifficultyCost;

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
