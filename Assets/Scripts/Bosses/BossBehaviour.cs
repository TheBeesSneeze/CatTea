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
    public int StartHealth;
    //public int CurrentHealth;

    [HideInInspector] public BossRoom MyRoom;

    // Start is called before the first frame update
    protected override void Start()
    {
        MaxHealthPoints = StartHealth; //yeah
        base.Start();
    }

    public override bool TakeDamage(int Damage)
    {
        return base.TakeDamage(Damage);
    }

    public override void Die()
    {
        Debug.Log("Boss die!");
        MyRoom.OnBossDeath();

        base.Die();

        //temp code hopefully
        Destroy(this.gameObject);
    }
}
