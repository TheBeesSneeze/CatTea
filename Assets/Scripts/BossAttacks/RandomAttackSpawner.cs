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

    //protected MyAnimator ratbossAnimator;

    public LayerMask LM;

    //protected override void Start()
    //{
    //    ratbossAnimator = GetComponent<MyAnimator>();
    //}
    public override void PerformAttack()
    {
        //ratbossAnimator.SetTrigger("FireMortar");

        Vector2 randomPosition = BossAttackUtilities.GetRandomPosition((Vector2)transform.position, AttackDistance, LM);
        GameObject newBomb = Instantiate(AttackPrefab, randomPosition, Quaternion.identity);

        bossBehaviour.AttacksSpawned.Add(newBomb);

        animator = bossBehaviour.MyAnimator;
        animator.SetTrigger("FireMortar");
    }

    
}

