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

    [Tooltip("This should be assigned automatically in the door's script.")]
    [HideInInspector] public DoorManager Door;

    public Transform CameraCenterPoint;
    public Transform PlayerSpawnPoint;

    protected PlayerBehaviour playerBehaviour;
    protected CameraManager cameraManager;

    public virtual void Start()
    {
        roomCleared = OpenDoorsOnStart;
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
    }

    /// <summary>
    /// initializes everything in the room
    /// </summary>
    public virtual void EnterRoom()
    {
        cameraManager.MoveCamera(CameraCenterPoint);
        
        playerBehaviour.transform.position = PlayerSpawnPoint.transform.position;

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
}
