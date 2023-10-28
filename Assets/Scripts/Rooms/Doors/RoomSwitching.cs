/*******************************************************************************
* File Name :         RoomSwitching.cs
* Author(s) :         Aiden Vangeberg, Toby Schamberger
* Creation Date :     9/30/2023
*
* "Brief" Description : 
* 
* TODO:
* Make inherit from door type 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSwitching : DoorManager
{
    [Tooltip("Rooms the door can lead too")]
    public List<RoomType> Rooms = new List<RoomType>();

    // Start is called before the first frame update
    protected override void Start()
    {
        DecideOutputRoom();

        base.Start();
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

        OutputRoom = Rooms[Random.Range(0, Rooms.Count)];
    }
}
