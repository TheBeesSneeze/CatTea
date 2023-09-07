using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    public EnemyStats CurrentEnemyStats;

    [Header("Don't touch these in editor")]
    public int Damage;
    protected int contactDamage;
    protected bool dealContactDamage;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

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

    public override void SetStatsToDefaults()
    {
        Speed = CurrentEnemyStats.Speed;
        HealthPoints = CurrentEnemyStats.HealthPoints;
        Damage = CurrentEnemyStats.Damage;
        contactDamage = CurrentEnemyStats.ContactDamage;
        dealContactDamage = CurrentEnemyStats.DealContactDamage;    

        KnockbackForce = CurrentEnemyStats.KnockBackForce;
    }
}
