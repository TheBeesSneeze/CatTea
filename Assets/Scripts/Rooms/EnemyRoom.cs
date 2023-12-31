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
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRoom : RoomType
{
    [Header("Settings")]
    public AudioClip RoomClearedMusic;

    public bool SpawnUpgradeOnCompletion=true;

    public List<GameObject> EnemySpawnPool;
    public List<Transform> EnemySpawnPoints;

    public int MinimumWaves = 3;
    public int MaximumWaves = 5;

    [Header("Debug")]
    [SerializeField] private int aliveEnemies;
    [SerializeField] private int wavesLeft;
    [SerializeField] private int challengePointsPerWave;

    //Secret settings
    protected float secondsUntilWaveStart = 2; 
    private float secondsBetweenEnemySpawns = 1f;
    protected float secondsForEnemyToSpawn = 2.5f;

    //magic numbers
    public float shadowExpandingScale = 3; // t^x (this is x)

    private int challengePointsLeft;
    private bool currentlySpawningEnemies;

    public override void EnterRoom()
    {
        base.EnterRoom();

        wavesLeft = Random.Range(MinimumWaves, MaximumWaves + 1);
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
        Debug.Log(aliveEnemies + " alive enemies...\n" + challengePointsLeft + " challenge points\n" + wavesLeft + " waves left");
        return (aliveEnemies <= 0 && challengePointsLeft <= 0 && wavesLeft <= 0);
    }

    /// <summary>
    /// When enemies die, they call this function.
    /// </summary>
    public virtual void OnEnemyDeath()
    {
        aliveEnemies--;
        Debug.Log(aliveEnemies + "enemies left");

        if (currentlySpawningEnemies)
            return;

        //spawn a new wave!
        if (wavesLeft > 0 && aliveEnemies <= 2)
        {
            //wavesLeft--;
            currentlySpawningEnemies = true;

            StartCoroutine(SpawnNewWaveOfEnemies());
        }

        //end everything!
        if (wavesLeft <= 0 && aliveEnemies <=0)
        {
            OnRoomClear();
            return;
        }
    }


    private void OnRoomClear()
    {
        Debug.Log("All enemies died! Opening door");
        Door.OpenDoor();

        if(SpawnUpgradeOnCompletion)
            Instantiate(UniversalVariables.Instance.UpgradeCollectionPrefab, PlayerBehaviour.Instance.transform.position, Quaternion.identity);

        if(RoomClearedMusic != null)
        {
            GameManager.Instance.TransitionMusic(RoomClearedMusic);
            return;
        }

        StopPlayingBackgroundMusic();
    }

    public virtual IEnumerator SpawnNewWaveOfEnemies()
    {
        currentlySpawningEnemies = true;

        yield return new WaitForSeconds(secondsUntilWaveStart);

        challengePointsLeft += challengePointsPerWave;

        Debug.Log("New wave! Using " + challengePointsLeft);

        List<Transform> spawnPointsAvailable = new List<Transform>(EnemySpawnPoints);

        while (challengePointsLeft > 0 && spawnPointsAvailable.Count > 0)
        {
            Transform spawnPoint = GetSpawnPoint(ref spawnPointsAvailable);
            StartCoroutine(SpawnEnemyShadow(spawnPoint));
            SpawnOneEnemy(spawnPoint.position);

            yield return new WaitForSeconds(secondsBetweenEnemySpawns);
        }

        challengePointsLeft = 0;
        currentlySpawningEnemies = false;

        wavesLeft--;
    }

    /// <summary>
    /// gets spawn point and removes it from list of available spawn points.
    /// 
    /// </summary>
    /// <returns>Trans</returns>
    public Transform GetSpawnPoint(ref List<Transform> spawnPointsAvailable)
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
    private void SpawnOneEnemy(Vector3 spawnPoint)
    {// my first ref use!! 9/16/2023
        int randomIndex = Random.Range(0, EnemySpawnPool.Count);

        SpawnEnemyByPoolIndex(randomIndex, spawnPoint);
    }

    /// <summary>
    /// Does not assume if enemy can be afforded. 
    /// </summary>
    protected void SpawnEnemyByPoolIndex(int Index, Vector3 spawnPoint)
    {
        //Debug.Log(challengePointsLeft);

        if(spawnPoint == null)
        {
            challengePointsLeft = 0;
            Debug.LogWarning("Not enough spawn points");
            return;
        }

        //instantiate new enemy
        StartCoroutine(SpawnEnemyDelay(Index, spawnPoint));
    }

    private IEnumerator SpawnEnemyDelay(int Index, Vector3 spawnPoint)
    {
        yield return new WaitForSeconds(secondsForEnemyToSpawn);

        GameObject EnemyPrefab = EnemySpawnPool[Index];
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint, Quaternion.identity);

        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        newEnemyBehaviour.OnSpawn();
        newEnemyBehaviour.Room = this;

        aliveEnemies++;

        int cost = GetEnemyCost(newEnemy);
        challengePointsLeft -= cost;
    }

    /// <summary>
    /// Returns enemies difficulty cost from pool index :)
    /// </summary>
    protected int GetEnemyCost(GameObject Enemy)
    {
        //move this function to roomtype if it needs it
        EnemyBehaviour enemyBehaviour = Enemy.GetComponent<EnemyBehaviour>();

        int cost = enemyBehaviour.CurrentEnemyStats.DifficultyCost;

        //Debug.Log(cost);

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
        Vector3 shadowRotation = UniversalVariables.Instance.EnemySpawningShadowPrefab.transform.eulerAngles;

        GameObject shadow = Instantiate(UniversalVariables.Instance.EnemySpawningShadowPrefab, shadowSpawnPoint.position, shadowSpawnPoint.transform.rotation);
        SpriteRenderer shadowSprite = shadow.GetComponent<SpriteRenderer>();

        shadow.transform.eulerAngles = shadowRotation;

        float targetShadowOpacity = shadowSprite.color.a;
        Vector3 targetShadowTransform = shadow.transform.localScale;

        Color startShadowColor = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, 0);
        Color targetShadowColor = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, shadowSprite.color.a);
        
        shadowSprite.color = startShadowColor;
        shadow.transform.localScale = Vector3.zero;

        float t = 0; // 0 -> secondsForEnemyToSpawn
        while (t < secondsForEnemyToSpawn)
        {
            t += Time.deltaTime;

            float tScaled = Mathf.Pow((t/ secondsForEnemyToSpawn), shadowExpandingScale);
            
            shadowSprite.color = Color.Lerp(startShadowColor, targetShadowColor, tScaled);
            shadow.transform.localScale = Vector3.Lerp(Vector3.zero, targetShadowTransform, tScaled);

            yield return null;
        }

        Destroy(shadow);
    }

    /// <summary>
    /// kills every enemy
    /// </summary>
    public override void Cheat()
    {
        base.Cheat();

        challengePointsLeft = 0;
        wavesLeft = 0;

        currentlySpawningEnemies = false;

        EnemyBehaviour[] allEnemies = GameObject.FindObjectsOfType<EnemyBehaviour>();

        foreach(EnemyBehaviour enemy in allEnemies)
        {
            enemy.Die();
        }

        OnRoomClear();
    }
}
