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
    public GameObject attack;
    public GameObject warningZone;

    protected float rotationModifier = 90;

    public float TimeBeforeDissapearing;
    public float TimeBeforeAttacking;
    public float AttackPlayerDistance = 3;
    public int AmountOfAttacks;

    private bool enemyVisible;

    //magic numbers
    private float secondsToToggleInvisibility = 0.5f;

    private Coroutine attackCoroutine;
    
   
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemyVisible = true;

        StartCoroutine(RotateEnemy());
        StartCoroutine(Dissapear());
        StartCoroutine(StartAttack());
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

            Color myColor = spriteRenderer.color;
            myColor.a = 1-t;

            spriteRenderer.color = myColor;
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

            Color myColor = spriteRenderer.color;
            myColor.a = t;

            spriteRenderer.color = myColor;
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
                if(attackCoroutine == null && !enemyVisible)
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
            warningZone.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            warningZone.SetActive(false);
            attack.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            attack.SetActive(false);
        }

        StartCoroutine(Dissapear());
        attackCoroutine = null;
    }
}
