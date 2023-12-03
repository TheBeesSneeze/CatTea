/*******************************************************************************
* File Name :         BombEnemyBehaviour.cs
* Author(s) :         Toby Schamberger, Aiden Vandeberg
* Creation Date :     
*
* Brief Description : 
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombEnemyBehaviour : EnemyBehaviour
{
    public GameObject bomb;
    public GameObject explosion;
    public int amountOfBombs;
    public int bombSpawnInterval;
    public float SecondsUntilExplode=1f;
    public float SecondsAfterExplode=0.5f;
    public bool bombSpawned;
    private List<GameObject> listOfBombs = new List<GameObject>();
    private List<GameObject> listOfExplosions = new List<GameObject>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        bombSpawned = false;
        StartCoroutine(SpawnBombs());
    }

    private IEnumerator SpawnBombs()
    {
        yield return new WaitForSeconds(bombSpawnInterval);

        for (int i = 0; i < amountOfBombs; i++)
        {
            Vector3 positionAroundPlayer = PlayerBehaviour.Instance.transform.position;
            Vector3 randomPosition = Random.insideUnitCircle;
            positionAroundPlayer.x += randomPosition.x;
            positionAroundPlayer.y += randomPosition.y;
            listOfBombs.Add(Instantiate(bomb, positionAroundPlayer, Quaternion.identity));
        }
        StartCoroutine(Explode());
        
       
        
        
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(SecondsUntilExplode);
        for(int i = 0; i < amountOfBombs; i++)
        {
            GameObject newBomb = Instantiate(explosion, listOfBombs[i].transform.position, Quaternion.identity);
            listOfExplosions.Add(newBomb);
            AttacksSpawned.Add(newBomb);

            Destroy(listOfBombs[i]);
        }

        yield return new WaitForSeconds(SecondsAfterExplode);
        for(int i = 0; i < amountOfBombs; i++)
        {
            Destroy(listOfExplosions[i]);
        }
        listOfBombs.Clear();
        listOfExplosions.Clear();
        //AttacksSpawned.Clear();
        StartCoroutine(SpawnBombs());
        
    }

    

    /*
    public override void Die()
    {
        SecondsUntilExplode = 0;
        SecondsAfterExplode = 0;

        for (int i = 0; i < listOfBombs.Count; i++)
        {
            Destroy(listOfBombs[i]);
        }

        for (int i = 0; i < listOfExplosions.Count; i++)
        {
            Destroy(listOfExplosions[i]);
        }

        base.Die();
    }
    */
}
