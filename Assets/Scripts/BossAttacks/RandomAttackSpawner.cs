/*******************************************************************************
* File Name :         RandomAttackSpawner.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/26/2023
*
* Brief Description : Shoots raycasts from the gameobject. chooses a random 
* point between that ray.
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttackSpawner : BossAttackType
{
    public float AttackDistance;

    public GameObject AttackPrefab;

    //protected Animator ratbossAnimator;

    public LayerMask LM;

    //protected override void Start()
    //{
    //    ratbossAnimator = GetComponent<Animator>();
    //}
    public override void PerformAttack()
    {
        //ratbossAnimator.SetTrigger("FireMortar");

        Vector2 randomPosition = BossAttackUtilities.GetRandomPosition((Vector2)transform.position, AttackDistance, LM);
        Instantiate(AttackPrefab, randomPosition, Quaternion.identity);
    }

    
}

