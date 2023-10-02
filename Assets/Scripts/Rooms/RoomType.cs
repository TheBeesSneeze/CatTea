/*******************************************************************************
* File Name :         RoomType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/13/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    protected bool roomCleared;
    //public bool RoomLoaded;
    public bool OpenDoorsOnStart;
    [Tooltip("If true, camera will follow the player")]
    public bool CameraFollowPlayer;
    [Tooltip("Zooms in and out")]
    public float CameraSize = 5;

    [Tooltip("This should be assigned automatically in the door's script.")]
    [HideInInspector] public DoorManager Door;

    [HideInInspector] public RoomSwitching EnemyDoor;

    public Transform CameraCenterPoint;
    public Transform PlayerSpawnPoint;

    protected PlayerBehaviour playerBehaviour;
    protected CameraManager cameraManager;
    protected GameManager gameManager;

    protected GameObject enemySpawningShadow;

    public virtual void Start()
    {
        roomCleared = OpenDoorsOnStart;
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// initializes everything in the room
    /// </summary>
    public virtual void EnterRoom()
    {
        gameManager.CurrentRoom = this;

        cameraManager.MoveCamera(CameraCenterPoint);
        
        playerBehaviour.transform.position = PlayerSpawnPoint.transform.position;
        Camera.main.orthographicSize = CameraSize;

        if (CameraFollowPlayer)
            cameraManager.StartFollowPlayer();
    }

    /// <summary>
    /// returns true/false
    /// </summary>
    public virtual bool CheckRoomCleared()
    {
        Debug.LogWarning("Override this function!");
        return roomCleared;
    }

    /// <summary>
    /// I really just don't want to have to get the *same* prefab every single time we make a room
    /// </summary>
    private void GetEnemyShadowCircle()
    {
        enemySpawningShadow = (GameObject)Resources.Load("prefabs/prefab1", typeof(GameObject));
    }

    /// <summary>
    /// Kills all the enemies or something
    /// </summary>
    public virtual void Cheat()
    {
        roomCleared = true;
        OpenDoorsOnStart = true;
    }
}
