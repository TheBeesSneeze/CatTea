using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyBehaviour : EnemyBehaviour
{
    private GameObject player;
    public float speed;
    public float rotationModifier;
    public GameObject dogAttack;
    private int wavesFired;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
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
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
            yield return null;
        }
        
    }

    private IEnumerator Attack()
    {
        while(this.gameObject != null)
        {
            float playerXDistance = player.transform.position.x - transform.position.x;
            float playerYDistance = player.transform.position.y - transform.position.y;
            if ((playerXDistance <= 4 && playerXDistance >= -4) && (playerYDistance <= 4 && playerYDistance >= -4))
            {
                if(wavesFired < 3)
                {
                    StartCoroutine(SpawnWaves());
                    yield return new WaitForSeconds(6);
                    wavesFired += 3;
                }
            }
            if(wavesFired >= 3)
            {
                yield return new WaitForSeconds(5);
                wavesFired = 0;
            }

            yield return null;
        }
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2);
        Instantiate(dogAttack, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);
        Instantiate(dogAttack, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);
        Instantiate(dogAttack, transform.position, transform.rotation);
        yield return null;
    }
}
