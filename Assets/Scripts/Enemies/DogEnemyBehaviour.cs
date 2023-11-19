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

    public int AttacksPerWave;
    public float TimeBetweenAttacks;
    public float AttackVelocity;
    public float AttackPlayerDistance = 7;

    //magic numbers
    protected float rotationModifier = 90;

    //private bool spawnWavesStarted;
    //private bool wavesLaunched;
    private Coroutine waveCoroutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(RotateEnemy());
        StartCoroutine(Attack());
    }

    private IEnumerator RotateEnemy()
    {
        while(this.gameObject != null)
        {
            Vector3 vectorToTarget = player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Speed);

            yield return null;
        }
        
    }

    private IEnumerator Attack()
    {
        while(this.gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);

            if (distanceToPlayer <= AttackPlayerDistance)
            {
                if(waveCoroutine == null)
                {
                    waveCoroutine = StartCoroutine(SpawnWaves());
                }
            }

            yield return null;
        }
    }

    protected virtual IEnumerator SpawnWaves()
    {
        for(int i =0; i< AttacksPerWave; i++)
        {
            yield return new WaitForSeconds(TimeBetweenAttacks);
            GameObject newAttack = Instantiate(dogAttackPrefab, transform.position, transform.rotation);

            AttacksSpawned.Add(newAttack);

            Vector2 dif = (player.transform.position - this.transform.position);
            newAttack.GetComponent<Rigidbody2D>().velocity = dif.normalized * AttackVelocity;
        }
        waveCoroutine = null;
    }
}
