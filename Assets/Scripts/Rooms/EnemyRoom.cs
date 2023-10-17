/*******************************************************************************
* File Name :         BossRoom.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/13/2023
*
* Brief Description : Stores reference to all enemies. Spawns enemies with 
* challenge points (Check documentation).
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRoom : RoomType
{
    [SerializeField] private List<GameObject> EnemySpawnPool;
    [SerializeField] private List<Transform> EnemySpawnPoints;

    [Header("Debug")]
    [SerializeField] private int aliveEnemies;
    [SerializeField] private int wavesLeft;
    [SerializeField] private int challengePointsPerWave;

    //Secret settings
    private int minWaves=3;
    private int maxWaves = 5;
    protected float secondsUntilWaveStart = 2; 
    private float secondsBetweenEnemySpawns = 1f;

    //magic numbers
    private float shadowExpandingFrames = 40;
    public float shadowExpandingScale = 3; // t^x (this is x)

    //le sound
    public AudioSource AccessGranted;

    public override void EnterRoom()
    {
        base.EnterRoom();

        wavesLeft = Random.Range(minWaves, maxWaves+1);
        challengePointsPerWave = GameManager.Instance.CurrentChallengePoints / wavesLeft;

        Debug.Log(wavesLeft + " waves, " + challengePointsPerWave + " challenge points per wave");

        if (EnemySpawnPool.Count <= 0)
        {
            Debug.LogWarning("No enemies in the pool!");
            return;
        }
        //else
        StartCoroutine(SpawnNewWaveOfEnemies());

    }

    public override bool CheckRoomCleared()
    {
        return (aliveEnemies <= 0);
    }

    /// <summary>
    /// When enemies die, they call this function.
    /// </summary>
    public virtual void OnEnemyDeath()
    {
        aliveEnemies--;

        if(aliveEnemies <= 0)
        {
            OnWaveEnd();
        }
    }

    private void OnWaveEnd()
    {
        wavesLeft--;

        if (wavesLeft <= 0)
        {
            Debug.Log("All enemies died! Opening door");
            Door.OpenDoor();
            AccessGranted.Play();
            return;
        }

        StartCoroutine(SpawnNewWaveOfEnemies());
    }

    public virtual IEnumerator SpawnNewWaveOfEnemies()
    {
        yield return new WaitForSeconds(secondsUntilWaveStart);

        int challengePointsLeft = challengePointsPerWave;

        Debug.Log("New wave! Using " + challengePointsLeft);

        aliveEnemies = 0;

        List<Transform> spawnPointsAvailable = new List<Transform>(EnemySpawnPoints);

        while (challengePointsLeft > 0)
        {
            Transform spawnPoint = GetSpawnPoint(ref challengePointsLeft, ref spawnPointsAvailable);
            StartCoroutine(SpawnEnemyShadow(spawnPoint));

            yield return new WaitForSeconds(secondsBetweenEnemySpawns);

            SpawnOneEnemy(ref challengePointsLeft, spawnPoint.position);
        }
    }

    /// <summary>
    /// gets spawn point and removes it from list of available spawn points.
    /// 
    /// </summary>
    /// <returns>Trans</returns>
    public Transform GetSpawnPoint(ref int challengePointsLeft, ref List<Transform> spawnPointsAvailable)
    {
        if (spawnPointsAvailable.Count <= 0)
        {
            Debug.LogWarning("Not enough enemy spawn spots");
            challengePointsLeft = 0;
            return null;
        }

        int transformIndex = Random.Range(0, spawnPointsAvailable.Count);
        Transform newPos = spawnPointsAvailable[transformIndex];
        spawnPointsAvailable.RemoveAt(transformIndex);

        return newPos;
    }

    /// <summary>
    /// Randomly selects an enemy from the pool. 
    /// 
    /// if first enemy randomly chosen cant be afforded. enemy pool is iterated 
    /// until first enemy that can be afforded is purchased.
    /// 
    /// if NO enemies can be afforded. challengePointsLeft is set to 0 and the 
    /// function quits
    /// </summary>
    private void SpawnOneEnemy(ref int challengePointsLeft, Vector3 spawnPoint)
    {// my first ref use!! 9/16/2023

        Debug.Log("Spawning one enemy");

        int randomIndex = Random.Range(0, EnemySpawnPool.Count);

        SpawnEnemyByPoolIndex(randomIndex, ref challengePointsLeft, spawnPoint);
    }

    /// <summary>
    /// Does not assume if enemy can be afforded. 
    /// </summary>
    protected void SpawnEnemyByPoolIndex(int Index, ref int ChallengePointsLeft, Vector3 spawnPoint)
    {
        Debug.Log(ChallengePointsLeft);

        if(spawnPoint == null)
        {
            ChallengePointsLeft = 0;
            Debug.LogWarning("Not enough spawn points");
            return;
        }
      
        //instantiate new enemy
        GameObject EnemyPrefab = EnemySpawnPool[Index];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint, Quaternion.identity);

        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        newEnemyBehaviour.OnSpawn();
        newEnemyBehaviour.Room = this;

        aliveEnemies++;

        int cost = GetEnemyCost(newEnemy);
        ChallengePointsLeft -= cost;
    }

    /// <summary>
    /// Returns enemies difficulty cost from pool index :)
    /// </summary>
    protected int GetEnemyCost(GameObject Enemy)
    {
        //move this function to roomtype if it needs it
        EnemyBehaviour enemyBehaviour = Enemy.GetComponent<EnemyBehaviour>();

        int cost = enemyBehaviour.CurrentEnemyStats.DifficultyCost;

        Debug.Log(cost);

        if (cost <= 0)
        {
            Debug.LogWarning(Enemy.name + " has a difficulty cost of zero! Don't do this a million enemies will spawn");
            cost = 1;
        }

        return cost;
    }

    /// <summary>
    /// Makes a shadow expand and fade opacity over secondsBetweenEnemySpawns.
    /// Dest
    /// </summary>
    public IEnumerator SpawnEnemyShadow(Transform shadowSpawnPoint)
    {
        GameObject shadow = Instantiate(UniversalVariables.Instance.EnemySpawningShadowPrefab, shadowSpawnPoint.position, Quaternion.identity);
        SpriteRenderer shadowSprite = shadow.GetComponent<SpriteRenderer>();

        float targetShadowOpacity = shadowSprite.color.a;
        Vector3 targetShadowTransform = shadow.transform.localScale;

        Color startShadowColor = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, 0);
        Color targetShadowColor = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, shadowSprite.color.a);
        
        shadowSprite.color = startShadowColor;
        shadow.transform.localScale = Vector3.zero;

        float t = 0; // 0 -> 1
        while(t < 1)
        {
            t += 1 / shadowExpandingFrames;
            float tScaled = Mathf.Pow(t, shadowExpandingScale);
            
            shadowSprite.color = Color.Lerp(startShadowColor, targetShadowColor, tScaled);
            shadow.transform.localScale = Vector3.Lerp(Vector3.zero, targetShadowTransform, tScaled);

            yield return new WaitForSeconds(secondsBetweenEnemySpawns/ shadowExpandingFrames);
        }

        Destroy(shadow);
    }

    /// <summary>
    /// kills every enemy
    /// </summary>
    public override void Cheat()
    {
        base.Cheat();

        EnemyBehaviour[] allEnemies = GameObject.FindObjectsOfType<EnemyBehaviour>();

        foreach(EnemyBehaviour enemy in allEnemies) 
        {
            enemy.Die();
        }
    }
}
