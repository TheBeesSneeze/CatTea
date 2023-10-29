/*******************************************************************************
* File Name :         BossAttackType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/28/2023
*
* Brief Description : General, static functions that can be used by any script.
* Functions are intended for BossAttackTypes but it does not need to be
*
* i've always wanted to make a utilities class!
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackUtilities : MonoBehaviour
{
    /// <summary>
    /// shoots a raycast in a random direction and gets a random point along it
    /// </summary>
    public static Vector2 GetRandomPosition(Vector2 centerPoint, float maxDistance, LayerMask LM)
    {
        //Vector2 direction = (Vector2)Random.rotation.eulerAngles;
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        direction.Normalize();

        //Debug.Log("Direction_is_" + direction);

        float randomPercent = Random.Range(0.25f, 0.75f);
        RaycastHit2D hitInfo = Physics2D.Raycast(centerPoint, direction, maxDistance * randomPercent, LM);

        if (hitInfo.transform != null)
        {
            Debug.DrawLine(centerPoint, hitInfo.point, Color.blue, 1);
            return hitInfo.point;
        }
        else
        {
            Debug.DrawLine(centerPoint, (Vector2)centerPoint + (direction * (10 * randomPercent)), Color.red, 1);
            return (Vector2)centerPoint + (direction * (10 * randomPercent));
        }
    }
}
