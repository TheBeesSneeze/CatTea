using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEnemyBehaviour : EnemyBehaviour
{
    private GameObject player;
    public float speed;
    public float rotationModifier;
    public GameObject wolfProjectile;
    public bool spawnProjectileStarted;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(RotateEnemy());
        StartCoroutine(LongRangedAttack());
    }

    

    private IEnumerator RotateEnemy()
    {
        while (this.gameObject != null)
        {
            Vector3 vectorToTarget = player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
            yield return null;
        }

    }

    private IEnumerator LongRangedAttack()
    {
        while(this.gameObject != null)
        {
            float playerXDistance = player.transform.position.x - transform.position.x;
            float playerYDistance = player.transform.position.y - transform.position.y;
            if ((playerXDistance <= 6 && playerXDistance >= -6) && (playerYDistance <= 6 && playerYDistance >= -6))
            {
                if(spawnProjectileStarted == false)
                {
                    StartCoroutine(SpawnProjectile());
                    spawnProjectileStarted = true;
                }
            }
            if(spawnProjectileStarted == true)
            {
                yield return new WaitForSeconds(4);
                spawnProjectileStarted = false;
            }
            yield return null;
        }
    }

    

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(1);
        Instantiate(wolfProjectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(wolfProjectile, transform.position, transform.rotation);
        yield return null;
    }
}
