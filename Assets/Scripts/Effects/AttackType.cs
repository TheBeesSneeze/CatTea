/*******************************************************************************
* File Name :         AttackType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/7/2023
*
* Brief Description : Base class for attacks and projectiles.
* By default, attacks things by colliding with them.
* 
* Automatically determines the attack source based off the GameObject tag. 
* 
* Uses two different player attack types because of different knockback values
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackType : MonoBehaviour
{
    [Header("Settings")]

    public int Damage;

    public float KnockbackForce;

    [Tooltip("If characters will take damage when colliding with the effect")]
    public bool DamageOnCollision = true;

    [Tooltip("If true, the effect will be destroyed by colliders with the 'Wall' tag")]
    public bool DestroyedByWalls = true;

    [Tooltip("If true, gameobject will be destroyed after attacking something")]
    public bool DestroyedAfterAttack = true;

    [Tooltip("If true, this will destoy other attacks (that are marked as DestoryedAfterAttack)")]
    public bool DestroyOtherAttacks = false;

    [Tooltip("If true, may be destroyed by other attacks")]
    public bool DestroyedByOtherAttacks = true;

    [Tooltip("If this # is less than 0, the attack will not be destroyed. Otherwise, destroys this gameobject")]
    public float DestroyAttackAfterSeconds = -1;

    public enum AttackSource { General, Enemy, Player};
    //public enum PlayerAttack { NA, Primary, Secondary };

    public AttackSource Attacker;
    //public PlayerAttack PlayerAttackType;

    protected virtual void Start()
    {
        DetermineAttackOwner();

        if (DestroyAttackAfterSeconds > 0)
            StartCoroutine(DestroyAfterSeconds());
    }

    /// <summary>
    /// Decides if this attack belongs to player or enemy and assigns
    /// result to EnemyAttack.
    /// Defaults to EnemyAttack = true;
    /// </summary>
    protected void DetermineAttackOwner()
    {
        if (tag.Equals("General Attack"))
        {
            Attacker = AttackSource.General;
            return;
        }

        //i do not like else if
        if (tag.Equals("Enemy Attack"))
        {
            Attacker = AttackSource.Enemy;
            return;
        }

        if (tag.Equals("Primary Player Attack") || tag.Equals("Secondary Player Attack"))
        {
            Attacker = AttackSource.Player;
            //PlayerAttackType = PlayerAttack.Primary;
            return;
        }

        //else:
        Debug.Log("No attack tag assigned for " + gameObject.name);
        Attacker = AttackSource.General;
    }

    public virtual void OnPlayerCollision(Collider2D collision)
    {
        if(Attacker.Equals(AttackSource.Enemy) || Attacker.Equals(AttackSource.General))
        {
            PlayerBehaviour player = collision.GetComponent<PlayerBehaviour>();

            player.TakeDamage(Damage, this.transform.position, KnockbackForce);

            if (DestroyedAfterAttack)
                Destroy(this.gameObject);
        }
    }

    public virtual void OnEnemyCollision(Collider2D collision)
    {
        if (Attacker.Equals(AttackSource.Player) || Attacker.Equals(AttackSource.General))
        {
            EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();

            enemy.TakeDamage(Damage, this.transform.position, KnockbackForce);

            if (DestroyedAfterAttack)
                Destroy(this.gameObject);
        }
    }

    public virtual void OnBossCollision(Collider2D collision)
    {
        if (Attacker.Equals(AttackSource.Player))
        {
            BossBehaviour boss = collision.GetComponent<BossBehaviour>();

            boss.TakeDamage(Damage, this.transform.position, KnockbackForce);

            if (DestroyedAfterAttack)
                Destroy(this.gameObject);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if(tag.Equals("Wall") && DestroyedByWalls)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
            return;
        }

        if(tag.Equals("Player"))
        {
            OnPlayerCollision(collision);
            return;
        }

        if(tag.Equals("Enemy"))
        {
            OnEnemyCollision(collision);
            return;
        }

        if (tag.Equals("Boss"))
        {
            OnBossCollision(collision);
            return;
        }

        //if(tag.Equals("Enemy Attack") || )
        {

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    public IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(DestroyAttackAfterSeconds);

        Destroy(this.gameObject);
    }
}
