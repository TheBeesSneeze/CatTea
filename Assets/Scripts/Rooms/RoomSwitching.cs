using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitching : MonoBehaviour
{
    [Tooltip("Room the door leads too")]
    public RoomType OutputRoom;
    public RoomType OutputRoom2;
    public RoomType OutputRoom3;
    [Tooltip("Room the door is on")]
    public RoomType ThisRoom;

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

        open = ThisRoom.OpenDoorsOnStart;

        ThisRoom.EnemyDoor = this;
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
        if (OutputRoom == null)
        {
            Debug.LogWarning("No output room!");
            return;
        }

        if (!open)
            return;

        switch (RandomNumberGenerator())
        {
            case 0:
                OutputRoom.EnterRoom();
                break;
            case 1:
                OutputRoom2.EnterRoom();
                break;
            case 2:
                OutputRoom3.EnterRoom();
                break;
        }
        
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

    private int RandomNumberGenerator()
    {
        int randomNumber = Random.Range(0, 3);
        return randomNumber;
    }
}
