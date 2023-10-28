/*******************************************************************************
* File Name :         RingOfProjectilesAttack.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/11/2023
*
* Brief Description : Shoots raycasts from the gameobject. chooses a random 
* point between that ray.
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfProjectilesAttack : BossAttackType
{
    public int ProjectilesInRing;
    public float ProjectileSpeed;
    public GameObject AttackPrefab;

    public override void PerformAttack()
    {

    }
}
