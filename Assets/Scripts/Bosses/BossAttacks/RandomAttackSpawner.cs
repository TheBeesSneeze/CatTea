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
        while (CurrentlyAttacking)
        {
            int attacks = 0;
            while (attacks < AttacksPerCycle)
            {
                attacks++;

                GetRandomPosition();
                //Instantiate(AttackPrefab, playerBehaviour.transform);

                yield return new WaitForSeconds(AttackInterval);
            }
            yield return new WaitForSeconds(AttackCycleCooldown);
        }
    }

    /// <summary>
    /// shoots a raycast in a random direction and gets a random point along it
    /// </summary>
    private void GetRandomPosition()
    {
        Vector3 direction = Random.rotation.eulerAngles;
        //Ray seeker = new Ray(this.transform.position, new Vector3(0, direction.y, 0));
        Ray seeker = new Ray(this.transform.position, direction);
        Debug.Log("Direction_is_" + direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(seeker, out hitInfo))
        {
            GameObject aITarget = hitInfo.collider.gameObject;
            Debug.Log("AI_chooses_" + aITarget.name);
        }
    }
}

