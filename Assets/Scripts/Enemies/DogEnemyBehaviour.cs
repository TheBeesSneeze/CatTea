/*******************************************************************************
* File Name :         DogEnemyBehaviour.cs
* Author(s) :         Aiden Vandeberg
* Creation Date :     10/4/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogEnemyBehaviour : EnemyBehaviour
{
    [Header("Wolf Settings:")]
    public GameObject dogAttackPrefab;

    public Transform Pivot;

    public int AttacksPerWave;
    public float TimeBetweenAttacks;
    public float TimeAfterAttack;
    public float AttackVelocity;
    public float AttackPlayerDistance = 7;
    public float AimRotationSpeed;

    //magic numbers
    protected float rotationModifier = 90;

    //private bool spawnWavesStarted;
    //private bool wavesLaunched;
    private Coroutine waveCoroutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(RotatePivot());
        StartCoroutine(Attack());
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
    

    private IEnumerator Attack()
    {
        while(this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, PlayerBehaviour.Instance.transform.position);

            if (distanceToPlayer <= AttackPlayerDistance)
            {
                if(waveCoroutine == null)
                {
                    waveCoroutine = StartCoroutine(SpawnBullets());
                }
            }

            yield return null;
        }
    }

    protected virtual IEnumerator SpawnBullets()
    {
        for(int i =0; i< AttacksPerWave; i++)
        {
            yield return new WaitForSeconds(TimeBetweenAttacks);

            GameObject newAttack = Instantiate(dogAttackPrefab, Pivot.transform.position, Pivot.transform.rotation);

            AttacksSpawned.Add(newAttack);

            Vector2 dif = (PlayerBehaviour.Instance.transform.position - this.transform.position);
            newAttack.GetComponent<Rigidbody2D>().velocity = dif.normalized * AttackVelocity;
        }
        yield return new WaitForSeconds(TimeAfterAttack);
        waveCoroutine = null;
    }
}
