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
    [Header("Important boss stuff")]
    [Tooltip("All the bosses which could be spawned")]
    public List<GameObject> BossPool;
    public Transform BossSpawnPosition;

    private GameObject bossObject;
    private BossBehaviour bossScript;
    private bool bossDead;

    public override void Start()
    {
        base.Start();
    }

    public override void EnterRoom()
    {
        GameEvents.Instance.OnRoomEnter();

        //copy and paste the code from roomType
        cameraManager.MoveCamera(CameraCenterPoint);

        playerBehaviour.transform.position = PlayerSpawnPoint.transform.position;
        Camera.main.orthographicSize = CameraSize;

        if (CameraFollowPlayer)
            cameraManager.StartFollowPlayer();

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
        bossDead = false;
    }

    public void OnBossDeath()
    {
        bossDead = true;
        roomCleared = true;

        if(Door != null)
            Door.OpenDoor();
    }

    public override bool CheckRoomCleared()
    {
        return (bossDead);
    }

}
