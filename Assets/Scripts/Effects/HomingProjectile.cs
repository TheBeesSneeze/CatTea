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

    public float TimeUntilDestroyed;
    [Tooltip("Percent 0-1. 0 for perfect aim. 1 for the worst")]
    public float Angle;

    [Header("Debug only:")]
    [SerializeField]private Transform target;

    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();

        if(Attacker.Equals(AttackSource.Enemy) || Attacker.Equals(AttackSource.General))
            target = GameObject.FindAnyObjectByType<PlayerBehaviour>().transform;

        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Home());
        StartCoroutine(SelfDestruct());
    }

    public IEnumerator Home()
    {
        while(this.gameObject != null)
        {
            Vector2 positionDifference = target.transform.position - transform.position;
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
