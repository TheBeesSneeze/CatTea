/*******************************************************************************
* File Name :         HomingProjectile.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/6/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingProjectile : AttackType
{
    public Transform Target;
    public float Speed;

    public float TimeUntilDestroyed;
    [Tooltip("Percent 0-1. 0 for perfect aim. 1 for the worst")]
    public float Angle;

    private Rigidbody2D rb;

    private void Start()
    {
        if(EnemyAttack)
            Target = GameObject.FindAnyObjectByType<PlayerBehaviour>().transform;

        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Home());
        StartCoroutine(SelfDestruct());
    }

    public IEnumerator Home()
    {
        while(this.gameObject != null)
        {
            Vector2 positionDifference = Target.transform.position - transform.position;
            positionDifference.Normalize();
            rb.velocity = positionDifference * Speed;

            //rb.transform.position = Vector2.MoveTowards()

            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(TimeUntilDestroyed);
        Destroy(this.gameObject);
    }
}
