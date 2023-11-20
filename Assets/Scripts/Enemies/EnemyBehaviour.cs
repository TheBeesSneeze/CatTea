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
using UnityEngine.AI;

public class EnemyBehaviour : CharacterBehaviour
{
    public EnemyStats CurrentEnemyStats;

    [HideInInspector] public int DifficultyCost;
    [HideInInspector] public int Damage;
    [HideInInspector] public bool EnemyMove; // if enemy should be stationary or nah
    [HideInInspector] public float TimeBetweenMovements;
    [HideInInspector] protected int contactDamage;
    [HideInInspector] protected bool dealContactDamage;
    [HideInInspector] public float MaxDistanceToPlayer;

    [HideInInspector] public EnemyRoom Room;

    protected Vector2 enemyDirection;
    protected GameObject player;

    protected override void Start()
    {
        base.Start();
        transform.eulerAngles = Vector3.zero;
        player = PlayerBehaviour.Instance.gameObject;

        StartCoroutine(UpdateAnimation());
    }

    /// <summary>
    /// Runs when enemy is spawned in.
    /// </summary>
    public virtual void OnSpawn()
    {
        //TODO
    }

    public override bool TakeDamage(float damage)
    {
        return TakeDamage(damage, true);
    }

    public override bool TakeDamage(float damage, bool onDamageEvent)
    {
        if (HealthPoints - damage > 0 && onDamageEvent)
            GameEvents.Instance.OnEnemyDamage(this);

        return base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();

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
        if(dealContactDamage)
            PlayerBehaviour.Instance.TakeDamage(contactDamage, this.transform.position, 0);
    }

    /// <summary>
    /// Calls various collision functions
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            OnPlayerCollision(collision);
        }
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

        MaxHealthPoints = CurrentEnemyStats.HealthPoints;
        HealthPoints = CurrentEnemyStats.HealthPoints;
        
        Damage = CurrentEnemyStats.Damage;
        contactDamage = CurrentEnemyStats.ContactDamage;
        dealContactDamage = CurrentEnemyStats.DealContactDamage;

        MaxDistanceToPlayer = CurrentEnemyStats.MaxDistanceFromPlayer;

        KnockbackForce = CurrentEnemyStats.KnockBackForce;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.speed = Speed;
        }
    }

    protected virtual IEnumerator UpdateAnimation()
    {
        while (true)
        {
            enemyDirection.x = GetComponent<NavMeshAgent>().velocity.x;
            enemyDirection.y = GetComponent<NavMeshAgent>().velocity.y;

            //Debug.Log(enemyDirection);

            MyAnimator.SetFloat("XMovement", enemyDirection.x);
            MyAnimator.SetFloat("YMovement", enemyDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
