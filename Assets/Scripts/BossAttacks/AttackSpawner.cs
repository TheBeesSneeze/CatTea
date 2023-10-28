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

    public override void PerformAttack()
    {
        Instantiate(AttackPrefab, playerBehaviour.transform);
    }
}
