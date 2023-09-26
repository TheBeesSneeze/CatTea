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

    public override void EnterRoom()
    {
        base.EnterRoom();

        wavesLeft = Random.Range(3, 6);
        challengePointsPerWave = GameManager.Instance.CurrentChallengePoints / wavesLeft;

        Debug.Log(wavesLeft + " waves, " + challengePointsPerWave + " challenge points per wave");

        SpawnNewWaveOfEnemies();
    }

    public override bool CheckRoomCleared()
    {
        return (aliveEnemies <= 0);
    }

    public virtual void SpawnNewWaveOfEnemies()
    {
        int challengePointsLeft = challengePointsPerWave;

        Debug.Log("New wave! Using " + challengePointsLeft);

        aliveEnemies = 0;

        if(EnemySpawnPool.Count <= 0)
        {
            Debug.LogWarning("No enemies in the pool!");
            return;
        }

        List<Transform> spawnPointsAvailable = new List<Transform>(EnemySpawnPoints);

        while(challengePointsLeft > 0)
        {
            SpawnOneEnemy(ref challengePointsLeft, ref spawnPointsAvailable);
        }
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
            return;
        }

        SpawnNewWaveOfEnemies();
    }

    /// <summary>
    /// Randomly selects an enemy from the pool. 
    /// 
    /// if first enemy randomly chosen cant be afforded. enemy pool is iterated 
    /// until first enemy that can be afforded is purchased.
    /// 
    /// if NO enemies can be afforded. ChallengePointsLeft is set to 0 and the 
    /// function quits
    /// </summary>
    private void SpawnOneEnemy(ref int ChallengePointsLeft, ref List<Transform>SpawnPointsAvailable)
    {// my first ref use!! 9/16/2023

        Debug.Log("Spawning one enemy");

        if(SpawnPointsAvailable.Count <= 0) 
        {
            Debug.LogWarning("Not enough enemy spawn spots");
            ChallengePointsLeft = 0;
            return;
        }
        
        int randomIndex = Random.Range(0, EnemySpawnPool.Count);

        SpawnEnemyByPoolIndex(randomIndex, ref ChallengePointsLeft, ref SpawnPointsAvailable);
    }

    /// <summary>
    /// Does not assume if enemy can be afforded. 
    /// </summary>
    protected void SpawnEnemyByPoolIndex(int Index, ref int ChallengePointsLeft, ref List<Transform> SpawnPointsAvailable)
    {
        Debug.Log(ChallengePointsLeft);

        if(SpawnPointsAvailable.Count <= 0)
        {
            ChallengePointsLeft = 0;
            Debug.LogWarning("Not enough spawn points");
            return;
        }

        //get spawn point (remove it from available spots)
        int transformIndex = Random.Range(0, SpawnPointsAvailable.Count);
        Vector3 enemySpawnPoint = SpawnPointsAvailable[transformIndex].position;
        SpawnPointsAvailable.RemoveAt(transformIndex);

        //instantiate new enemy
        GameObject EnemyPrefab = EnemySpawnPool[Index];
        GameObject newEnemy = Instantiate(EnemyPrefab, enemySpawnPoint, Quaternion.identity);

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
