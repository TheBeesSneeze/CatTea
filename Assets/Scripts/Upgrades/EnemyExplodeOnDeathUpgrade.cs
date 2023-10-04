/*******************************************************************************
* File Name :         EnemyExplodeOnDeathUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/4/2023
*
* Brief Description : Spawns an explosion when enemies die
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplodeOnDeathUpgrade : UpgradeType
{
    public GameObject ExplosionPrefab;

    public override void UpgradeEffect(Vector3 eventPosition)
    {
        Instantiate(ExplosionPrefab, eventPosition, Quaternion.identity);
    }
}
