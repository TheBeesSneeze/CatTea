/*******************************************************************************
* File Name :         BossRoom.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/13/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : EnemyRoom
{
    [Tooltip("All the bosses which could be spawned")]
    public List<GameObject> BossPool;
    public Transform BossSpawnPosition;

    private GameObject bossObject;
    private BossBehaviour bossScript;

    public override void Start()
    {
        base.Start();
    }

    public override void EnterRoom()
    {
        //copy and paste the code from roomType
        cameraManager.MoveCamera(CameraCenterPoint);
        playerBehaviour.transform.position = PlayerSpawnPoint.transform.position;

        SpawnBoss();
    }

    /// <summary>
    /// TODO: animation or something
    /// </summary>
    private void SpawnBoss()
    {
        if(BossPool.Count <= 0)
        {
            Debug.LogWarning("Boss room has no available bosses");
            return;
        }

        int randomBossIndex = Random.Range(0, BossPool.Count);

        bossObject = Instantiate(BossPool[randomBossIndex], BossSpawnPosition.position, Quaternion.identity);
        bossScript = bossObject.GetComponent<BossBehaviour>();
        bossScript.MyRoom = this;
    }

    public void OnBossDeath()
    {
        Door.OpenDoor();
    }

}
