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
    private bool open;

    private void Start()
    {
        open = ThisRoom.OpenDoorsOnStart;
    }

    /// <summary>
    /// Sets open to true. Animation stuff should be here.
    /// </summary>
    /// <returns></returns>
    public void OpenDoor()
    {
        open = true;
    }

    /// <summary>
    /// Sends player to next room (OutputRoom)
    /// </summary>
    public void EnterDoor()
    {
        if (!open)
            return;

        if(OutputRoom == null)
        {
            Debug.LogWarning("No output room!");
            OutputRoom.EnterRoom();
        }
            
    }
}
