/*******************************************************************************
* File Name :         DoorManager.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/13/2023
*
* Brief Description : can be set to open/closed. stores this room and an output room.
* 
* TODO: animations, probably
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Tooltip("Room the door leads too")]
    public RoomType OutputRoom;
    [Tooltip("Room the door is on")]
    public RoomType ThisRoom;

    [Tooltip("If player can go through the door")]
    [SerializeField] protected bool open;

    //gross... unity...
    private PlayerBehaviour playerBehaviour;

    private void Start()
    {
        playerBehaviour = GameObject.FindAnyObjectByType<PlayerBehaviour>();

        if(ThisRoom == null)
        {
            Debug.LogWarning("No Room Assigned to door!");
            return;
        }

        open = ThisRoom.OpenDoorsOnStart;

        ThisRoom.Door = this;
    }

    /// <summary>
    /// Sets open to true. Animation stuff should be here.
    /// </summary>
    /// <returns></returns>
    public void OpenDoor()
    {
        open = true;
        //TODO: ANIMATION STUFF
    }

    /// <summary>
    /// Sends player to next room (OutputRoom)
    /// </summary>
    public virtual void EnterDoor()
    {
        if(OutputRoom == null)
        {
            Debug.LogWarning("No output room!");
            return;
        }

        if(!open)
            return;
        
        OutputRoom.EnterRoom();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag.Equals("Player"))
        {
            AttemptEnterDoor();
        }
    }

    /// <summary>
    /// Tries to let player open door
    /// </summary>
    protected virtual void AttemptEnterDoor()
    {
        if (OutputRoom == null)
            return;

        open = ThisRoom.CheckRoomCleared();

        EnterDoor();
    }
}
