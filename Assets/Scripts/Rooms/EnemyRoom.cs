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

    public int AliveEnemies;
    public int WavesLeft;
    private int challengePointsPerWave;

    public override void EnterRoom()
    {
        base.EnterRoom();

        WavesLeft = Random.Range(3, 6);
        challengePointsPerWave = GameManager.Instance.CurrentChallengePoints / WavesLeft;

        Debug.Log(WavesLeft + " waves, " + challengePointsPerWave + " challenge points per wave");

        SpawnNewWaveOfEnemies();
    }

    public override bool CheckRoomCleared()
    {
        return (AliveEnemies <= 0);
    }

    public virtual void SpawnNewWaveOfEnemies()
    {
        int challengePointsLeft = challengePointsPerWave;
        AliveEnemies = 0;

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
        AliveEnemies--;

        if (AliveEnemies <= 0)
        {
            Debug.Log("All enemies died! Opening door");
            Door.OpenDoor();
        }
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
        if(SpawnPointsAvailable.Count <= 0) 
        {
            Debug.LogWarning("Not enough enemy spawn spots");
            ChallengePointsLeft = 0;
            return;
        }
        
        int randomIndex = Random.Range(0, EnemySpawnPool.Count);

        int cost = GetEnemyCostByPoolIndex(randomIndex);

        //if enemy can be afforded
        if(cost <= ChallengePointsLeft)
        {
            SpawnEnemyByPoolIndex(randomIndex, ref ChallengePointsLeft, ref SpawnPointsAvailable);
            return;
        }

        //if first random enemy couldnt be afforded
        for(int i=0; i<EnemySpawnPool.Count; i++)
        {
            cost = GetEnemyCostByPoolIndex(i);

            if (cost <= ChallengePointsLeft)
            {
                SpawnEnemyByPoolIndex(i, ref ChallengePointsLeft, ref SpawnPointsAvailable);
                return;
            }
        }

        //this code should ideally run:
        Debug.Log("Not enough enemies for ChallengePoints");
        ChallengePointsLeft = 0;
        return;
    }

    /// <summary>
    /// Does not assume if enemy can be afforded. 
    /// </summary>
    protected void SpawnEnemyByPoolIndex(int Index, ref int ChallengePointsLeft, ref List<Transform> SpawnPointsAvailable)
    {
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

        AliveEnemies++;
        ChallengePointsLeft -= newEnemyBehaviour.DifficultyCost;
    }

    /// <summary>
    /// Returns enemies difficulty cost from pool index :)
    /// </summary>
    protected int GetEnemyCostByPoolIndex(int Index)
    {
        //move this function to roomtype if it needs it
        GameObject Enemy = EnemySpawnPool[Index];
        EnemyBehaviour enemyBehaviour = Enemy.GetComponent<EnemyBehaviour>();
        return enemyBehaviour.DifficultyCost;
    }
}
