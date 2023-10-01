/*******************************************************************************
* File Name :         RoomSwitching.cs
* Author(s) :         Aiden Vangeberg, Toby Schamberger
* Creation Date :     9/30/2023
*
* "Brief" Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSwitching : MonoBehaviour
{
    [Tooltip("Rooms the door can lead too")]
    public List<RoomType> Rooms = new List<RoomType>();

    [Tooltip("Room the door is on")]
    public RoomType ThisRoom;

    [HideInInspector] public RoomType OutputRoom;

    [Header("Debug")]

    [Tooltip("If player can go through the door")]
    [SerializeField] protected bool open;

    //gross... unity...
    private PlayerBehaviour playerBehaviour;

    // Start is called before the first frame update
    private void Start()
    {
        playerBehaviour = GameObject.FindAnyObjectByType<PlayerBehaviour>();

        if (ThisRoom == null)
        {
            Debug.LogWarning("No Room Assigned to door!");
            return;
        }

        DecideOutputRoom();

        open = ThisRoom.OpenDoorsOnStart;

        ThisRoom.EnemyDoor = this;
    }

    /// <summary>
    /// Called in start, 
    /// </summary>
    private void DecideOutputRoom()
    {
        if (Rooms.Count == 0)
        {
            Debug.LogWarning("No output room!");

            OutputRoom = ThisRoom;
            return;
        }

        OutputRoom = Rooms[Random.Range(0,Rooms.Count)];
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
    /// Sends player to next room (OutputRoom).
    /// Assumes door is open.
    /// </summary>
    public virtual void EnterDoor()
    {
        OutputRoom.EnterRoom();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
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
