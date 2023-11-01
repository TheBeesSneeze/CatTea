/*******************************************************************************
* File Name :         DissapearingEnemy.cs
* Author(s) :         Aiden Vandeberg, TobySchamberger
* Creation Date :     
*
* Brief Description : 
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DissapearingEnemy : EnemyBehaviour
{
    private GameObject player;
    public GameObject attack;

    protected Animator chameleonAnimator;

    protected Vector2 enemyDirection;

    protected float rotationModifier = 90;

    public float TimeBeforeDissapearing;
    public float TimeBeforeAttacking;
    public float AttackPlayerDistance = 3;
    public int AmountOfAttacks;
    public bool enemyVisible;

    //magic numbers
    private float secondsToToggleInvisibility = 0.5f;

    private Coroutine attackCoroutine;
    
   
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemyVisible = true;
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;

        //StartCoroutine(RotateEnemy());
        StartCoroutine(Dissapear());
        StartCoroutine(StartAttack());

        chameleonAnimator = GetComponent<Animator>();

        StartCoroutine(UpdateAnimation());
    }

    

    private IEnumerator RotateEnemy()
    {
        while (this.gameObject != null)
        {
            Vector3 vectorToTarget = player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Speed);

            yield return null;
        }

    }

    private IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(TimeBeforeDissapearing);

        float t = 0;
        while(t < 1)
        {
            t+= Time.deltaTime / secondsToToggleInvisibility;

            Color myColor = mySpriteRenderer.color;
            myColor.a = 1-t;

            mySpriteRenderer.color = myColor;
        }

        yield return new WaitForSeconds(TimeBeforeAttacking);
        enemyVisible = false;
        
    }

    private IEnumerator Appear()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / secondsToToggleInvisibility;

            Color myColor = mySpriteRenderer.color;
            myColor.a = t;

            mySpriteRenderer.color = myColor;
        }

        yield return new WaitForSeconds(TimeBeforeAttacking);
        enemyVisible = true;
    }

    private IEnumerator StartAttack()
    {
        while(this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);

            if(distanceToPlayer <= AttackPlayerDistance)
            {
                if(attackCoroutine == null && enemyVisible == false)
                {
                    attackCoroutine = StartCoroutine(Attacking());
                }
            }
            yield return null;
        }
    }

    private IEnumerator Attacking()
    {
        
        StartCoroutine(Appear());

        for (int i = 0; i < AmountOfAttacks; i++)
        {
            yield return new WaitForSeconds(0.3f);
            attack.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            attack.SetActive(false);
        }
        
        attackCoroutine = null;
        StartCoroutine(Dissapear());
        
    }

    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            enemyDirection.x = GetComponent<NavMeshAgent>().velocity.x;
            enemyDirection.y = GetComponent<NavMeshAgent>().velocity.y;

            //Debug.Log(enemyDirection);

            chameleonAnimator.SetFloat("XMovement", enemyDirection.x);
            chameleonAnimator.SetFloat("YMovement", enemyDirection.y);
            yield return null;
        }
    }
}
