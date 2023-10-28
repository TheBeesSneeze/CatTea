using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingEnemy : EnemyBehaviour
{
    private GameObject player;
    public GameObject attack;
    protected float rotationModifier = 90;

    public float TimeBeforeDissapearing;
    public float TimeBeforeAttacking;
    public float AttackPlayerDistance = 3;
    public int AmountOfAttacks;
    public bool enemyVisible;

    private Coroutine attackCoroutine;
    
   
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemyVisible = true;
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
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
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(TimeBeforeAttacking);
        enemyVisible = false;
        
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
        enemyVisible = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
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
}
