using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : EnemyBehaviour
{
    private GameObject player;
    public GameObject attack;

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
            yield return new WaitForSeconds(1);
            attack.SetActive(true);
            canRotate = false;
            yield return new WaitForSeconds(1);
            attack.SetActive(false);
            canRotate = true;
        }
        yield return new WaitForSeconds(TimeBeforeAttacking);
        attackingCoroutine = null;
    }
}
