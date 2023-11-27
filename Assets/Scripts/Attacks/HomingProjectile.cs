/*******************************************************************************
* File Name :         HomingProjectile.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/6/2023
*
* Brief Description : If attack is from enemy, homes towards player.
* If attack is from player, finds the closest enemy and homes towards them. 
* The target does not change after it is decided.
* 
* TODO: player mode
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingProjectile : AttackType
{
    public float Speed;

    [Tooltip("Percent 0-1. 1 for perfect aim. 0 for no aim")]
    public float Angle;

    [Header("Debug only:")]
    [SerializeField]private Transform target;

    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();

        if(Attacker.Equals(AttackSource.Enemy) || Attacker.Equals(AttackSource.General))
            target = PlayerBehaviour.Instance.transform;

        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Home());
    }

    public IEnumerator Home()
    {
        rb.velocity = GetTargetVelocity();

        while(this.gameObject != null)
        {
            Vector2 targetVelocity = GetTargetVelocity();

            Vector2 newVelocity = Vector2.Lerp(rb.velocity, targetVelocity, Angle* Time.deltaTime);
            newVelocity.Normalize();

            rb.velocity = newVelocity * Speed;

            //rb.transform.position = Vector2.MoveTowards()

            yield return null;
        }
    }

    private Vector2 GetTargetVelocity()
    {
        Vector2 positionDifference = target.transform.position - transform.position;
        positionDifference.Normalize();
        return positionDifference * Speed;
    }
}
