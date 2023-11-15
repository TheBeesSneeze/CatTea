/*******************************************************************************
* File Name :         SwordEnemy.cs
* Author(s) :         Aiden Vandeberg
* Creation Date :     
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordEnemy : EnemyBehaviour
{
    private GameObject player;
    public GameObject attack;
    public GameObject warningzone;

    protected Animator stoatAnimator;

    protected Vector2 enemyDirection;

    protected float rotationModifier = 90;
    public float AttackPlayerDistance = 7;
    public int AmountOfAttacks;
    public float TimeBeforeAttacking;
    public bool canRotate;

    private Coroutine attackingCoroutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
        canRotate = true;
        StartCoroutine(RotateEnemy());
        StartCoroutine(Attack());

        stoatAnimator = GetComponent<Animator>();

        StartCoroutine(UpdateAnimation());
    }

    private IEnumerator RotateEnemy()
    {
        while (this.gameObject != null)
        {
            if(canRotate == true)
            {
                Vector3 vectorToTarget = player.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Speed);
            }
            
            yield return null;
        }

    }

    private IEnumerator Attack()
    {
        while (this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);

            if (distanceToPlayer <= AttackPlayerDistance)
            {
                if (attackingCoroutine == null)
                {
                    attackingCoroutine = StartCoroutine(Attacking());
                }
            }

            yield return null;
        }
    }

    private IEnumerator Attacking()
    {
        for (int i = 0; i < AmountOfAttacks; i++)
        {
            warningzone.SetActive(true);
            yield return new WaitForSeconds(1);
            warningzone.SetActive(false);
            attack.SetActive(true);
            canRotate = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            yield return new WaitForSeconds(1);
            attack.SetActive(false);
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            canRotate = true;
        }
        yield return new WaitForSeconds(TimeBeforeAttacking);
        attackingCoroutine = null;
    }

    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            enemyDirection.x = GetComponent<NavMeshAgent>().velocity.x;
            enemyDirection.y = GetComponent<NavMeshAgent>().velocity.y;

            //Debug.Log(enemyDirection);

            stoatAnimator.SetFloat("XMovement", enemyDirection.x);
            stoatAnimator.SetFloat("YMovement", enemyDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
