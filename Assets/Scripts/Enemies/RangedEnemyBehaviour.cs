using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBehaviour : EnemyBehaviour
{
    [Tooltip("How many bullets to spawn in a burst")]
    public int BulletsSpawned;

    [Tooltip("Seconds between each spawning of bullets")]
    public int BulletSpawnInterval;

    public GameObject BulletPrefab;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        while(this.gameObject != null) 
        { 
            yield return new WaitForSeconds(BulletSpawnInterval);

            for(int i=0; i< BulletsSpawned; i++)
            {
                Instantiate(BulletPrefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
            }
        }
    }


}
