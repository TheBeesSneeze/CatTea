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

    [Tooltip("Leave null for no music")]
    public AudioClip BackgroundMusic;

    [Tooltip("If null, doesnt change background color")]
    public Color BackgroundColor;

    //public bool RoomLoaded;
    public bool OpenDoorsOnStart;
    [Tooltip("If true, camera will follow the player")]
    public bool CameraFollowPlayer=true;
    [Tooltip("Zooms in and out")]
    public float CameraSize = 6;

    [Tooltip("This should be assigned automatically in the door's script.")]
    public DoorManager Door;

    [Header("Debug")]
    public bool ForceCloseDoorOverride=false;

    [HideInInspector] public RoomSwitching EnemyDoor;

    public Transform CameraCenterPoint;
    public Transform PlayerSpawnPoint;

    protected CameraManager cameraManager;
    protected AudioSource backgroundMusicPlayer;

    public virtual void Start()
    {
        backgroundMusicPlayer = GameObject.Find("Background Music").GetComponent<AudioSource>();

        roomCleared = OpenDoorsOnStart;
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
    }

    /// <summary>
    /// initializes everything in the room
    /// </summary>
    public virtual void EnterRoom()
    {
        GameEvents.Instance.OnRoomEnter();
        GameManager.Instance.CurrentRoom = this;

        StartPlayingBackgroundMusic();

        if(CameraCenterPoint != null && cameraManager != null)
            cameraManager.MoveCamera(CameraCenterPoint);
        
        PlayerBehaviour.Instance.transform.position = PlayerSpawnPoint.transform.position;
        Camera.main.orthographicSize = CameraSize;

        if (CameraFollowPlayer)
            cameraManager.StartFollowPlayer();
    }

    public virtual void ExitRoom()
    {
        StopPlayingBackgroundMusic();
    }

    /// <summary>
    /// returns true/false
    /// </summary>
    public virtual bool CheckRoomCleared()
    {
        //Debug.LogWarning("Override this function!");
        return roomCleared;
    }

    /// <summary>
    /// Kills all the enemies or something
    /// </summary>
    public virtual void Cheat()
    {
        PlayerBehaviour.Instance.HealthPoints = PlayerBehaviour.Instance.MaxHealthPoints;

        roomCleared = true;
        OpenDoorsOnStart = true;
        Door.OpenDoor(); 
    }

    public virtual void StartPlayingBackgroundMusic()
    {
        if (backgroundMusicPlayer == null)
            return;

        backgroundMusicPlayer.clip = BackgroundMusic;

        GameManager.Instance.ScaleBackgroundMusic(1);
        backgroundMusicPlayer.Play();
    }

    public virtual void StopPlayingBackgroundMusic()
    {
        //backgroundMusicPlayer.clip = null;
        GameManager.Instance.ScaleBackgroundMusic(0);
    }

}
