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
    [HideInInspector] protected float contactDamage;
    [HideInInspector] protected bool dealContactDamage;
    [HideInInspector] public float MaxDistanceToPlayer;

    [HideInInspector] public EnemyRoom Room;

    protected Vector2 enemyDirection;
    protected NavMeshAgent agent;

    private Coroutine knockbackCoroutine;
    private Coroutine contactDamageCoroutine;

    //magic numbers
    private float KnockBackSeconds = 0.13f; //how long the navmesh agent will be turned off
    private float SecondsBetweenContactDamage = 0.5f;

    protected override void Start()
    {
        transform.eulerAngles = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();

        base.Start();

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

    public override void KnockBack(GameObject target, Vector3 damageSourcePosition, float force)
    {
        if (force <= 0)
            return;

        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(KnockBackAgent(target, damageSourcePosition, force));
    }

    /// <summary>
    /// turns off/on navmesh agent 
    /// </summary>
    /// <returns></returns>
    private IEnumerator KnockBackAgent(GameObject target, Vector3 damageSourcePosition, float force)
    {
        if (agent == null)
            yield break;

        agent.enabled = false;
        myRigidbody2D.isKinematic = false;

        base.KnockBack(target, damageSourcePosition, force);

        yield return new WaitForSeconds(KnockBackSeconds);

        agent.enabled = true;
        myRigidbody2D.isKinematic = true;
        myRigidbody2D.velocity = Vector3.zero;
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
        if (dealContactDamage)
        {
            PlayerBehaviour.Instance.TakeDamage(contactDamage, this.transform.position, 0);
            contactDamageCoroutine = StartCoroutine(RefreshCollider());
        }
    }

    private IEnumerator RefreshCollider()
    {
        yield return new WaitForSeconds(SecondsBetweenContactDamage);

        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        collider.enabled = true;

        contactDamageCoroutine = null;
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

    public virtual void OnTriggerExit(Collider other)
    {
        if (tag.Equals("Player"))
        {
            if (contactDamageCoroutine != null)
                StopCoroutine(contactDamageCoroutine);
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

        if(agent != null)
        {
            agent.speed = Speed;
            agent.stoppingDistance = MaxDistanceToPlayer;
        }
    }

    protected virtual IEnumerator UpdateAnimation()
    {
        if (agent == null)
            yield break;
        
        while (true)
        {
            enemyDirection.x = agent.velocity.x;
            enemyDirection.y = agent.velocity.y;

            //Debug.Log(enemyDirection);

            MyAnimator.SetFloat("XMovement", enemyDirection.x);
            MyAnimator.SetFloat("YMovement", enemyDirection.y);

            yield return null;
        }
    }
}
