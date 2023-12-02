/*******************************************************************************
* File Name :         EnemyPathFinding.cs
* Author(s) :         Elda Osami, Toby Schamberger
* Creation Date :     9/24/2023
*
* Brief Description : Routes the enemies towards the player.
* 
* TODO: implement cycles of waiting.
* Ranged enemies should be distanced from player
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyPathFinding : MonoBehaviour
{
    private Vector3 target;
    private Transform player;
    private NavMeshAgent agent;
    private EnemyBehaviour enemyBehaviour;

    // Start is called before the first frame update
    void Awake()
    {
        player = PlayerBehaviour.Instance.transform;
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
     }

    private void Update()
    {
        SetTargetPoisiton();
        SetAgentPosition();
    }
  

    /// <summary>
    /// starts to inch away slightly if player gets too close
    /// </summary>
    private void SetTargetPoisiton()
    {
        target = player.position;

        /*
        float distance = Vector2.Distance(transform.position, target);

        if(distance >= enemyBehaviour.MaxDistanceToPlayer)
        {
            target = player.position;
            return;
        }

        //target = transform.position;

        target = transform.position + (transform.position-player.position);
        */
    }

    private void SetAgentPosition()
    {
        if(agent.enabled)
        {
            agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
        }
    }
}
