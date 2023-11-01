/*******************************************************************************
* File Name :         MovementCycle.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/28/2023
*
* Brief Description : Shoots raycasts from the gameobject. chooses a random 
* point between that ray.
* Moves guy there.
* Uses MoveUnitsPerSecond from BossBehaviour.cs
* *****************************************************************************/

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCycle : BossAttackType
{
    public float MaxMovementDistance;

    public LayerMask LM;

    private Coroutine moveCoroutine;

    public override void PerformAttack()
    {
        if (moveCoroutine != null)
            return;

        Vector2 randomPosition = BossAttackUtilities.GetRandomPosition((Vector2)transform.position, MaxMovementDistance, LM);

        //Debug.Log ("moving boss to " + randomPosition);

        moveCoroutine = StartCoroutine(MoveAcross(transform.position, randomPosition));
    }

    private IEnumerator MoveAcross(Vector2 startPoint, Vector2 endPoint)
    {
        float distance = Vector2.Distance(startPoint, endPoint);
        float totalSecondsToMove = distance / bossBehaviour.MoveUnitsPerSecond;

        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / totalSecondsToMove;

            Vector2 newPosition = Vector2.Lerp(startPoint, endPoint, t);

            transform.position = newPosition;

            Debug.DrawLine(startPoint, endPoint);

            yield return null;
        }

        moveCoroutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);

        if(layer.Equals("Level"))
        {
            GetOutOfWall(collision.transform.position);
        }
    }

    private void GetOutOfWall(Vector2 wallPosition)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        Vector2 difference = (Vector2)transform.position - wallPosition;
        Vector2 newTarget = difference.normalized * 10;

        moveCoroutine = StartCoroutine(MoveAcross(wallPosition, newTarget));
    }
}
