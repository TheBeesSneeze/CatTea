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

    [Tooltip("Seconds between waves of attacks")]
    public float AttackCycleCooldown;
    [Tooltip("Seconds between each attack gameobject being spawned")]
    public float AttackInterval;
    [Tooltip("# of attack objects spawned per cycle")]
    public float AttacksPerCycle;

    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(SpawnAttacks());
    }

    /// <summary>
    /// Spawns attacks at the player
    /// </summary>
    /// <returns></returns>
    protected IEnumerator SpawnAttacks()
    {
        while(CurrentlyAttacking)
        {
            int attacks = 0;
            while (attacks < AttacksPerCycle)
            {
                attacks++;

                Instantiate(AttackPrefab, playerBehaviour.transform);

                yield return new WaitForSeconds(AttackInterval);
            }
            yield return new WaitForSeconds(AttackCycleCooldown);
        }
    }
}
