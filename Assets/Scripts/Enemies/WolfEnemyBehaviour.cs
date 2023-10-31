/*******************************************************************************
* File Name :         DogEnemyBehaviour.cs
* Author(s) :         Aiden Vandeberg
* Creation Date :     10/4/2023
*
* Brief Description : sorry i deleted everything. i made it extend DogEnemyBehaviour
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WolfEnemyBehaviour : DogEnemyBehaviour
{
    protected Animator wolfAnimator;

    protected Vector2 enemyDirection;

    protected override void Start()
    {
        wolfAnimator = GetComponent<Animator>();

        StartCoroutine(UpdateAnimation());
    }
    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            enemyDirection.x = GetComponent<NavMeshAgent>().velocity.x;
            enemyDirection.y = GetComponent<NavMeshAgent>().velocity.y;

            //Debug.Log(enemyDirection);

            wolfAnimator.SetFloat("XMovement", enemyDirection.x);
            wolfAnimator.SetFloat("YMovement", enemyDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
