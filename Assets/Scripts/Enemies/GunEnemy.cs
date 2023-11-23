using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GunEnemy : EnemyBehaviour
{
    public GameObject firstAttack;
    public GameObject firstAttackWarningZone;
    public GameObject secondAttack;
    public GameObject secondAttackWarningZone;

    protected float rotationModifier = 90;
    public float AttackPlayerDistance = 7;
    public int AmountOfAttacks;

    private Coroutine attackingCoroutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(RotateEnemy());
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

    private IEnumerator StartAttack()
    {
        while (this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);

            if (distanceToPlayer <= AttackPlayerDistance)
            {
                if(attackingCoroutine == null)
                {
                    attackingCoroutine = StartCoroutine(Attacking());
                }
            }

            yield return null;
        }
    }

    private IEnumerator Attacking()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        for (int i = 0; i < AmountOfAttacks; i++)
        {
            switch (RandomAttack())
            {
                case 0:
                    firstAttackWarningZone.SetActive(true);
                    yield return new WaitForSeconds(1);
                    firstAttackWarningZone.SetActive(false);
                    firstAttack.SetActive(true);
                    yield return new WaitForSeconds(1);
                    firstAttack.SetActive(false);
                    break;
                case 1:
                    secondAttackWarningZone.SetActive(true);
                    yield return new WaitForSeconds(1);
                    secondAttackWarningZone.SetActive(false);
                    secondAttack.SetActive(true);
                    yield return new WaitForSeconds(1);
                    secondAttack.SetActive(false);
                    break;
            }
            
        }
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        yield return new WaitForSeconds(5);
        attackingCoroutine = null;
    }

    private int RandomAttack()
    {
        return Random.Range(0, 2);
    }

}
