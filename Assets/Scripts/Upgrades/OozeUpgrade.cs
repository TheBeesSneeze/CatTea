/*******************************************************************************
* File Name :         OozeUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/27/2023
*
* Brief Description : Activated on start. Spawns in evil goop puddles all around
* the player
* To change Ooze damage, go into ooze attack prefab.
* This code can be generalized as: spawns prefabs in random rangearound player
* 
* Ooze initialization is in OozeAttack.cs
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozeUpgrade : UpgradeType
{
    [Header("Ooze settings:")]
    public GameObject OozePrefab;

    public float MinSecondsBetweenOozePuddles;
    public float MaxSecondsBetweenOozePuddles;

    public float MaxSpawningDistance;

    public override void UpgradeEffect()
    {
        StartCoroutine(SpawnPuddles());
    }

    private IEnumerator SpawnPuddles()
    {
        

        while (this != null)
        {
            float secondsTilNextOoze = Random.Range(MinSecondsBetweenOozePuddles, MaxSecondsBetweenOozePuddles);

            yield return new WaitForSeconds(secondsTilNextOoze);

            Vector3 randomPosition = Random.insideUnitCircle * MaxSpawningDistance;
            randomPosition.y -= 0.2f;

            
            Instantiate(OozePrefab, PlayerBehaviour.Instance.transform.position + randomPosition, Quaternion.identity);

            //Ooze initialization is in OozeAttack
        }
    }
}
