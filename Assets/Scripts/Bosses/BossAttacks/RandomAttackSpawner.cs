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

    public LayerMask LM;

    public override void PerformAttack()
    {
        Vector2 randomPosition = GetRandomPosition();
        Instantiate(AttackPrefab, randomPosition, Quaternion.identity);
    }

    /// <summary>
    /// shoots a raycast in a random direction and gets a random point along it
    /// </summary>
    private Vector2 GetRandomPosition()
    {
        //Vector2 direction = (Vector2)Random.rotation.eulerAngles;
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        direction.Normalize();

        //Debug.Log("Direction_is_" + direction);

        float randomPercent = Random.Range(0.25f, 0.75f);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,10 * randomPercent, LM);

        if (hitInfo.transform != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.blue,1);
            return hitInfo.point;
        }
        else
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + (direction * (10 * randomPercent)), Color.red,1);
            return (Vector2)transform.position + (direction * (10 * randomPercent));
        }
    }
}

