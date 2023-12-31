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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCycle : BossAttackType
{
    public float MaxMovementDistance;

    public LayerMask LM;

    public float MaxDistanceFromPlayer = 15;

    [Header("Debug")]
    public bool DisableMovementOverride=false;

    private Coroutine moveCoroutine;

    private Vector2 startPosition;

    protected override void Start()
    {
        base.Start();
        startPosition=transform.position;
    }

    public override void PerformAttack()
    {
        if (moveCoroutine != null)
            return;

        if(Vector2.Distance(transform.position, PlayerBehaviour.Instance.transform.position) > MaxDistanceFromPlayer)
        {
            MoveTowardsPlayer();
            return;
        }

        MoveRandomly();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 anglePoint;
        Vector3 endPoint;

        Vector2 direction = PlayerBehaviour.Instance.transform.position - transform.position;
        direction.Normalize();

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerBehaviour.Instance.transform.position);
        float distance = Mathf.Min(distanceToPlayer, MaxMovementDistance);
        distance *= 0.9f;

        anglePoint = direction * (distance / 2);
        anglePoint = anglePoint + transform.position;

        endPoint = direction * distance;
        endPoint = endPoint + transform.position;

        moveCoroutine = StartCoroutine(MoveAcross(transform.position, anglePoint, endPoint));
    }

    private void MoveRandomly()
    {
        Vector2 anglePoint;
        Vector2 endPoint;

        float attempts = 0;

        do
        {
            attempts++;
            anglePoint = BossAttackUtilities.GetRandomPosition((Vector2)transform.position, MaxMovementDistance, LM);
        }
        while (Vector2.Distance((Vector2)transform.position, anglePoint) < 1 && attempts < 15);

        attempts = 0;
        do
        {
            attempts++;
            endPoint = BossAttackUtilities.GetRandomPosition(anglePoint, MaxMovementDistance, LM);
        }
        while (Vector2.Distance(anglePoint, endPoint) < 3 && attempts < 15);

        //Debug.Log ("moving boss to " + randomPosition);

        moveCoroutine = StartCoroutine(MoveAcross(transform.position, anglePoint, endPoint));
    }

    /// <summary>
    /// Moves the boss across using bezier curves (look it up)
    /// </summary>
    private IEnumerator MoveAcross(Vector2 startPoint, Vector2 anglePoint, Vector2 endPoint)
    {
        float distance = GetBezierLength(startPoint,anglePoint, endPoint);
        float totalSecondsToMove = distance / bossBehaviour.MoveUnitsPerSecond;

        float time = 0;
        while (time < totalSecondsToMove && bossBehaviour.HealthPoints > 0 && !DisableMovementOverride)
        {
            time += Time.deltaTime;
            float t = time / totalSecondsToMove; // 0 <= t <= 1

            Vector2 newPosition = BezierCurve(startPoint, anglePoint, endPoint, t);

            //Debug.Log("Distance: " + Vector2.Distance(transform.position, newPosition));

            transform.position = newPosition;

            Debug.DrawLine(startPoint, anglePoint,Color.yellow);
            Debug.DrawLine(anglePoint, endPoint, Color.yellow);

            yield return null;
        }

        moveCoroutine = null;
    }

    /// <summary>
    /// old, unused i think
    /// </summary>
    private IEnumerator MoveAcross(Vector2 startPoint, Vector2 endPoint)
    {
        float distance = Vector2.Distance(startPoint, endPoint);
        float totalSecondsToMove = distance / bossBehaviour.MoveUnitsPerSecond;

        float time = 0;
        while(time < totalSecondsToMove && !DisableMovementOverride)
        {
            time += Time.deltaTime;
            float t = time / totalSecondsToMove; // 0 <= t <= 1

            Vector2 newPosition = Vector2.Lerp(startPoint, endPoint, t);

            transform.position = newPosition;

            Debug.DrawLine(startPoint, endPoint);

            yield return null;
        }

        if (Vector2.Distance(startPosition, transform.position) > 40)
        {
            transform.position = startPosition;
        }

        moveCoroutine = null;
    }


    /// <summary>
    /// i love bezier curves. i really just wanted an excuse to put one in my code tbh
    /// </summary>
    private Vector2 BezierCurve(Vector2 start, Vector2 angle, Vector2 end, float t)
    {
        Vector2 a_b = Vector2.Lerp(start, angle, t);
        Vector2 b_c = Vector2.Lerp(angle, end, t);

        Vector2 m = Vector2.Lerp(a_b, b_c, t);

        Debug.DrawLine(a_b, b_c, Color.white);

        return m;
    }

    /// <summary>
    /// approximates the length of a bezier curve by measuring the distance
    /// of slices of a bezier curve
    /// </summary>
    private float GetBezierLength(Vector2 start, Vector2 angle, Vector2 end)
    {
        float totalDistance=0; 
        for(float t=0; t<10; t++)
        {
            Vector2 a = BezierCurve(start, angle, end, t/10);
            Vector2 b = BezierCurve(start, angle, end, (t+1)/10);

            totalDistance += Vector2.Distance(a, b);
        }

        return totalDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);

        if(layer.Equals("Level"))
        {
            //GetOutOfWall(collision.GetContact(0).point);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);

        if (layer.Equals("Level"))
        {
            Debug.Log("hey guys");
        }
    }


    private void GetOutOfWall(Vector2 wallPosition)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        Vector2 difference = (Vector2)transform.position - wallPosition;
        Vector2 newTarget = difference.normalized * 10;

        Debug.Log("New target: " + newTarget);

        moveCoroutine = StartCoroutine( MoveAcross(wallPosition, newTarget) );
    }
}
