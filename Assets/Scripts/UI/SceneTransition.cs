/*******************************************************************************
* File Name :         SceneTransition.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/27/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    public GameObject TransitionSquare;

    private GameObject player;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    public void SwitchRooms(RoomType currentRoom, RoomType nextRoom)
    {
        currentRoom.ExitRoom();

        nextRoom.EnterRoom();
    }
}
