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
    public int Damage;
    public float KnockbackForce;
    public bool DamageOnCollision = true;

    public enum AttackSource { General, Enemy, Player};
    //public enum PlayerAttack { NA, Primary, Secondary };

    public AttackSource Attacker;
    //public PlayerAttack PlayerAttackType;

    protected virtual void Start()
    {
        DetermineAttackOwner();
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
        }
    }

    public virtual void OnEnemyCollision(Collider2D collision)
    {
        if (Attacker.Equals(AttackSource.Player) || Attacker.Equals(AttackSource.General))
        {
            EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();

            enemy.TakeDamage(Damage, this.transform.position, KnockbackForce);
        }
    }

    public virtual void OnBossCollision(Collider2D collision)
    {
        if (Attacker.Equals(AttackSource.Player))
        {
            BossBehaviour boss = collision.GetComponent<BossBehaviour>();

            boss.TakeDamage(Damage, this.transform.position, KnockbackForce);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }
}
