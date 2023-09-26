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
    public GameObject player;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        StartCoroutine(UpdateTarget());
    }

    public virtual IEnumerator UpdateTarget()
    {
        //sorry for changing this. i just have personal beef with update
        while(this != null)
        {
            SetTargetPoisiton();
            SetAgentPosition();
            yield return null;
        }
        
    }


    private void SetTargetPoisiton()
    {
        target = player.transform.position;
    }

    private void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
