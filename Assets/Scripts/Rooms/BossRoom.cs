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
    private BossDialogue bossDialogue;
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

        PlayerBehaviour.Instance.transform.position = PlayerSpawnPoint.transform.position;
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
        bossDialogue = bossObject.GetComponent<BossDialogue>();

        bossScript.MyRoom = this;
        bossDead = false;

        if (bossDialogue == null)
        {
            OnBossTextEnded();
            return;
        }

        bossDialogue.Room = this;

        Door.CloseDoor();

        StartBossText();
    }

    private void StartBossText()
    {
        bossDialogue.Initialize();
        bossDialogue.ActivateSpeech();
    }

    public void OnBossTextEnded()
    {
        StartPlayingBackgroundMusic();

        bossScript.DialogueEnded = true;

        bossScript.Initialize();
    }

    public void OnBossDeath()
    {
        bossDead = true;
        roomCleared = true;

        if(Door != null)
            Door.OpenDoor();

        if (RoomClearedMusic != null)
        {
            GameManager.Instance.TransitionMusic(RoomClearedMusic);
            return;
        }

        StopPlayingBackgroundMusic();
    }

    public override bool CheckRoomCleared()
    {
        return (bossDead);
    }

    public override void Cheat()
    {
        if(bossScript != null) 
            bossScript.Die();

        base.Cheat();
    }
}
