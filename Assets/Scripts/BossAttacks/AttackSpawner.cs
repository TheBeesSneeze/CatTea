/*******************************************************************************
* File Name :         AttackSpawner.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/18/2023
*
* Brief Description : Spawns attacks at the players position.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawner : BossAttackType
{
    public GameObject AttackPrefab;

    //protected Animator ratbossAnimator;

    //protected override void Start()
    //{
    //    ratbossAnimator = GetComponent<Animator>();
    //}

    public override void PerformAttack()
    {
        //ratbossAnimator.SetTrigger("FireMortar");
        Instantiate(AttackPrefab, playerBehaviour.transform);
    }
}
