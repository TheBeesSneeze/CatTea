/*******************************************************************************
* File Name :         RangedEnemyBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/6/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyBehaviour : EnemyBehaviour
{
    public GameObject BulletPrefab;

    [Tooltip("How many bullets to spawn in a burst")]
    public int BulletsSpawned;

    [Tooltip("Seconds between each spawning of bullets")]
    public int BulletSpawnInterval;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        while(this.gameObject != null) 
        { 
            yield return new WaitForSeconds(BulletSpawnInterval);

            for(int i=0; i< BulletsSpawned; i++)
            {
                GameObject newBullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

                AttacksSpawned.Add(newBullet);

                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}
