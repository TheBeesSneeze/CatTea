/*******************************************************************************
* File Name :         BossBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/18/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : CharacterBehaviour
{
    public float StartHealth;
    public float MoveUnitsPerSecond;
    //public int CurrentHealth;

    [HideInInspector] public BossRoom MyRoom;
    protected PlayerBehaviour playerBehaviour;

    // Start is called before the first frame update
    protected override void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        MaxHealthPoints = StartHealth; //yeah
        base.Start();
    }

    public override bool TakeDamage(float damage)
    {
        return TakeDamage(damage,true);
    }

    public override bool TakeDamage(float damage, bool onDamageEvent)
    {
        if (HealthPoints - damage > 0 && onDamageEvent)
            GameEvents.Instance.OnEnemyDamage(this);

        return base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameEvents.Instance.OnEnemyDeath(this.transform.position);

        Debug.Log("Boss die!");
        MyRoom.OnBossDeath();

        base.Die();

        //temp code hopefully
        Destroy(this.gameObject);
    }
}
