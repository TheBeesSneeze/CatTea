/*******************************************************************************
* File Name :         MeleeEnemyBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/16/2023
*
* "Brief" Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    protected Animator bearAnimator;

    protected Vector2 enemyDirection;

    protected override void Start()
    {
        bearAnimator = GetComponent<Animator>();

        StartCoroutine(UpdateAnimation());
    }
    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            enemyDirection.x = GetComponent<NavMeshAgent>().velocity.x;
            enemyDirection.y = GetComponent<NavMeshAgent>().velocity.y;

            //Debug.Log(enemyDirection);

            bearAnimator.SetFloat("XMovement", enemyDirection.x);
            bearAnimator.SetFloat("YMovement", enemyDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
