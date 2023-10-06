using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyBehaviour : EnemyBehaviour
{
    private GameObject player;
    public GameObject bomb;
    public int amountOfBombs;
    public int bombSpawnInterval;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpawnBombs());
    }

    

    private IEnumerator SpawnBombs()
    {
        while (this.gameObject != null)
        {
            yield return new WaitForSeconds(bombSpawnInterval);

            for (int i = 0; i < amountOfBombs; i++)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                Vector3 positionAroundPlayer = Random.insideUnitCircle * player.transform.position;
                Instantiate(bomb, positionAroundPlayer, Quaternion.identity);
            }
        }
    }

    
}
