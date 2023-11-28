/*******************************************************************************
* File Name :         GunEnemy.cs
* Author(s) :         Aiden Vandeberg, Toby Schamberger
* Creation Date :     
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GunEnemy : EnemyBehaviour
{
    [Header("Gun Enemy Settings")]
    public GameObject BulletPrefab;

    public float AttackPlayerDistance;

    public float WarningSecondsUntilAttack;

    public int NumberOfBullets;
    public float SecondsBetweenBullets;
    public float SecondsBetweenAttacks;

    public float BulletSpeed;

    public float AimRotationSpeed;

    [Header("Unity Stuff")]
    public Transform Pivot;

    //public GameObject FirstAttack;
    public GameObject FirstAttackWarningZone;
    //public GameObject secondAttack;
    public GameObject SecondAttackWarningZone;

    protected float rotationModifier = 90;

    private Coroutine attackingCoroutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(RotatePivot());
        StartCoroutine(StartAttack());
    }

    private IEnumerator RotatePivot()
    {
        while (this.gameObject != null)
        {
            Vector3 vectorToTarget = PlayerBehaviour.Instance.transform.position - transform.position;

            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            Pivot.rotation = Quaternion.Slerp(Pivot.rotation, q, Time.deltaTime * AimRotationSpeed);

            yield return null;
        }

    }

    private IEnumerator StartAttack()
    {
        while (this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, PlayerBehaviour.Instance.transform.position);

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

        switch (RandomAttack())
        {
            case 0:
                Attack(FirstAttackWarningZone);
                break;
            case 1:
                Attack(SecondAttackWarningZone);
                break;
        }

        //time until the attack is done:
        yield return new WaitForSeconds((NumberOfBullets * SecondsBetweenBullets) + WarningSecondsUntilAttack);

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        FirstAttackWarningZone.SetActive(false);
        SecondAttackWarningZone.SetActive(false);

        yield return new WaitForSeconds(SecondsBetweenAttacks);
        attackingCoroutine = null;
    }

    /// <summary>
    /// spawns n bullets in the directions in attackWarningZone. 
    /// does vector math magic
    /// </summary>
    /// <param name="attackWarningZone"></param>
    /// <returns></returns>
    private void Attack(GameObject attackWarningZone)
    {
        attackWarningZone.SetActive(true);

        Transform[] bulletAngles = GetChildren(attackWarningZone.transform);

        for (int i =0; i< bulletAngles.Length; i++)
        {
            StartCoroutine(SpawnBullets(bulletAngles[i]));
        }
        

        /*
        FirstAttackWarningZone.SetActive(true);
        yield return new WaitForSeconds(1);
        FirstAttackWarningZone.SetActive(false);
        FirstAttack.SetActive(true);
        yield return new WaitForSeconds(1);
        FirstAttack.SetActive(false);
        */
    }

    private Transform[] GetChildren(Transform parent)
    {
        int n = parent.childCount;
        Transform[] children = new Transform[n];

        for(int i=0; i<n; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }

    private IEnumerator SpawnBullets(Transform direction)
    {
        yield return new WaitForSeconds(WarningSecondsUntilAttack);

        for (int i=0; i< NumberOfBullets; i++)
        {
            Vector2 angle = direction.position - transform.position;
            angle.Normalize();

            GameObject bullet = Instantiate(BulletPrefab, transform.position, direction.rotation);
            Rigidbody2D bulletRB= bullet.GetComponent<Rigidbody2D>();
            bulletRB.rotation += 90;

            bulletRB.velocity = angle* BulletSpeed;

            yield return new WaitForSeconds(SecondsBetweenBullets);
        }
    }

    // why???
    private int RandomAttack()
    {
        return Random.Range(0, 2);
    }

}
