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
    public Sprite OpenDoorSprite;
    public Sprite ClosedDoorSprite;

    [Tooltip("Room the door leads too")]
    public RoomType OutputRoom;
    [Tooltip("Room the door is on")]
    public RoomType ThisRoom;

    [Tooltip("If player can go through the door")]
    [SerializeField] protected bool open;

    //gross... unity...
    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(ThisRoom == null)
        {
            Debug.LogWarning("No Room Assigned to door!");
            return;
        }

        if(ThisRoom.OpenDoorsOnStart)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

        ThisRoom.Door = this;
    }

    /// <summary>
    /// Sets open to true. Animation stuff should be here.
    /// </summary>
    /// <returns></returns>
    public void OpenDoor()
    {
        if (ThisRoom.ForceCloseDoorOverride)
            return;

        open = true;
        //TODO: ANIMATION STUFF

        spriteRenderer.sprite = OpenDoorSprite;
    }

    public void CloseDoor()
    {
        open = false;

        spriteRenderer.sprite = ClosedDoorSprite;
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

        open = false;
        RoomTransition.Instance.SwitchRooms(ThisRoom, OutputRoom);
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
        {
            CloseDoor();
            return;
        }

        if (ThisRoom.ForceCloseDoorOverride)
        {
            CloseDoor();
            return;
        }

        open = ThisRoom.CheckRoomCleared();

        if (!open)
        {
            CloseDoor();
            return;
        }

        OpenDoor();
        EnterDoor();
    }
}
