/*******************************************************************************
* File Name :         DissapearingEnemy.cs
* Author(s) :         Aiden Vandeberg, TobySchamberger
* Creation Date :     
*
* Brief Description : Becomes invisible when close enough to player.
* when even closer to player, it becomes visible again and hurts you!
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DissapearingEnemy : EnemyBehaviour
{
    public float SpeedWhenInvisible;
    private float defaultSpeed;

    public GameObject attack;
    public GameObject warningZone;

    //public float TimeBeforeDissapearing;
    //public float TimeBeforeAttacking;

    public float BecomeInvisibleDistance = 15;
    public float AttackPlayerDistance = 0.1f;
    //public int AmountOfAttacks;

    private bool runningFromPlayer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        defaultSpeed = agent.speed;

        //StartCoroutine(RotateEnemy());
        //StartCoroutine(Dissapear());
        //(StartAttack());

        StartCoroutine(ChangeOpacityByPlayerDistance());
    }

    /// <summary>
    /// hating myself for using update but this is literally the only solution i could find and ive been working on this for 4 hours straight
    /// </summary>
    private void LateUpdate()
    {
        float distance = Vector2.Distance(this.transform.position, PlayerBehaviour.Instance.transform.position);
        distance = Mathf.Clamp(distance, AttackPlayerDistance, BecomeInvisibleDistance);

        float t = distance / BecomeInvisibleDistance;
        //t = Mathf.Pow(t, 0.2f);
        AdjustSpeed(t);

        if (distance > AttackPlayerDistance)
        {
            AdjustOpacity(t);
        }

        if (distance < AttackPlayerDistance)
        {
            float i = (AttackPlayerDistance - distance) / AttackPlayerDistance;
            AdjustOpacity(i);
        }
    }

    /// <summary>
    /// for some reason EVERYTHING breaks if i get rid of this coroutine so i guess its fucking staying
    /// </summary>
    private IEnumerator ChangeOpacityByPlayerDistance()
    {
        yield return null;
    }

    private void AdjustSpeed(float t)
    {
        if (runningFromPlayer)
            return;

        float tScaled = Mathf.Pow(t, 0.5f);

        agent.speed = Mathf.Lerp(SpeedWhenInvisible, defaultSpeed, tScaled);
    }

    private void AdjustOpacity(float t)
    {
        Color color = spriteRenderer.color;
        color.a = t;

        spriteRenderer.color = color;
    }

    protected override void OnPlayerCollision(Collider2D collision)
    {
        base.OnPlayerCollision(collision);

        StartCoroutine(RunFromPlayer());
    }

    private IEnumerator RunFromPlayer()
    {
        runningFromPlayer= true;
        agent.speed = -defaultSpeed;

        yield return new WaitForSeconds(3);

        agent.speed = defaultSpeed;
        runningFromPlayer = false;
    }

    /*
    private IEnumerator Dissapear()
    {
        enemyVisible = false;

        Debug.Log("disappearing");

        float time = 0;
        while(time < secondsToBecomeInvisible)
        {
            time+= Time.deltaTime;
            float t = time / secondsToBecomeInvisible;

            Color myColor = spriteRenderer.color;
            myColor.a = 1.05f-t;

            spriteRenderer.color = myColor;

            AdjustSpeedByOpacity();

            yield return null;
        }

        visibilityCoroutine = null;
    }

    private IEnumerator Appear()
    {
        enemyVisible = true;

        float time = 0;
        while (time < secondsToBecomeVisible)
        {
            time += Time.deltaTime;
            float t = time / secondsToBecomeVisible;

            Color myColor = spriteRenderer.color;
            myColor.a = t;

            spriteRenderer.color = myColor;

            AdjustSpeedByOpacity();

            yield return null;
        }

        visibilityCoroutine = null;
    }

    private void AdjustSpeedByOpacity()
    {
        float a = spriteRenderer.color.a;
        float aScaled = Mathf.Pow(a, 0.5f);

        agent.speed = Mathf.Lerp(SpeedWhenInvisible, defaultSpeed, a);
    }

    private IEnumerator ChangeOpacityByPlayerDistance()
    {
        while(true)
        {
            float distance = Vector2.Distance(this.transform.position, PlayerBehaviour.Instance.transform.position);

            if(AttackPlayerDistance < distance && distance < BecomeInvisibleDistance)
            {
                BecomeInvisible();
            }
            if(distance < AttackPlayerDistance || distance > BecomeInvisibleDistance)
            {
                BecomeVisible();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void BecomeInvisible()
    {
        if (!enemyVisible)
        {
            return;
        }

        if(visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine);
        }

        visibilityCoroutine = StartCoroutine(Dissapear());
    }

    private void BecomeVisible()
    {
        if (enemyVisible)
            return;

        if (visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine);
        }

        visibilityCoroutine = StartCoroutine(Appear());
    }
    */

    /*
    
     private IEnumerator RotateEnemy()
    {
        while (this.gameObject != null)
        {
            Vector3 vectorToTarget = PlayerBehaviour.Instance.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Speed);

            yield return null;
        }

    }

    private IEnumerator StartAttack()
    {
        while(this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, PlayerBehaviour.Instance.transform.position);

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
    */
}
