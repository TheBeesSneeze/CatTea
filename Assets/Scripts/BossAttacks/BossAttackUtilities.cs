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

        
        RaycastHit2D hitInfo = Physics2D.Raycast(centerPoint, direction, maxDistance, LM);

        if (hitInfo.transform != null)
        {
            Debug.DrawLine(centerPoint, hitInfo.point, Color.blue, 1);
            return CorrectRandomPositionOutput(centerPoint, hitInfo.point);
        }

        else //if it hit nothing:
        {
            Debug.DrawLine(centerPoint, (Vector2)centerPoint + (direction * (maxDistance * 0.5f)), Color.red, 1);
            return CorrectRandomPositionOutput(centerPoint, direction * maxDistance);
            //return (Vector2)centerPoint + (direction * (10 * randomPercent));
        }
    }

    /// <summary>
    /// returns output with account for random!
    /// </summary>
    private static Vector2 CorrectRandomPositionOutput(Vector2 centerPoint, Vector2 rayHitPoint)
    {
        float distance = Vector2.Distance(centerPoint, rayHitPoint);

        Vector2 direction = rayHitPoint - centerPoint;
        direction.Normalize();

        float randomPercent = Random.Range(0.25f, 0.75f);

        direction = direction * (distance * randomPercent);

        //return centerPoint + direction;

        //FUCK YOU. HERES THE FUNCTION AGAIN BUT ON ONE LINE. ALL THE ABOVE CODE WAS FOR NOTHING
        return centerPoint + ((rayHitPoint - centerPoint).normalized*(Vector2.Distance(centerPoint, rayHitPoint) * Random.Range(0.25f, 0.75f)));
    }
}
