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

    //protected MyAnimator ratbossAnimator;

    //protected override void Start()
    //{
    //    ratbossAnimator = GetComponent<MyAnimator>();
    //}

    public override void PerformAttack()
    {
        animator.SetTrigger("FireMortar");
        Instantiate(AttackPrefab, playerBehaviour.transform);
    }
}
