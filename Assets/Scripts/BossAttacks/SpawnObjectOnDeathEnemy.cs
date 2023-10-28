/*******************************************************************************
* File Name :         SpawnObjectOnDeathEnemy.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/27/2023
*
* Brief Description : Extends enemy behaviour. Spawns in an object when it dies.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnDeathEnemy : EnemyBehaviour
{
    public GameObject Prefab;

    public override void Die()
    {
        Instantiate(Prefab, this.transform.position, Quaternion.identity);

        base.Die();
    }
}
