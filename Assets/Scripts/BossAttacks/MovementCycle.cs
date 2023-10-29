/*******************************************************************************
* File Name :         MovementCycle.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/28/2023
*
* Brief Description : Shoots raycasts from the gameobject. chooses a random 
* point between that ray.
* Moves guy there.
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCycle : BossAttackType
{
    public float MaxMovementDistance;
    public float MovementSpeed;

    public LayerMask LM;

    private Coroutine moveCoroutine;

    public override void PerformAttack()
    {
        if (moveCoroutine != null)
            return;

        Vector2 randomPosition = BossAttackUtilities.GetRandomPosition((Vector2)transform.position, MaxMovementDistance, LM);

        moveCoroutine = StartCoroutine(MoveAcross(transform.position, randomPosition));
    }

    private IEnumerator MoveAcross(Vector2 startPoint, Vector2 endPoint)
    {
        float distance = Vector2.Distance(startPoint, endPoint);
        float totalSecondsToMove = distance * MovementSpeed;

        float t = 0;
        while(t < totalSecondsToMove)
        {
            t += Time.deltaTime;

            Vector2 newPosition = Vector2.Lerp(startPoint, endPoint, t/ totalSecondsToMove);

            transform.position = newPosition;

            yield return null;
        }
    }


}
