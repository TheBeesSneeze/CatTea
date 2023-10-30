/*******************************************************************************
* File Name :         SceneProgresser.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/26/2023
*
* Brief Description : This script will most likely go unused when the game is finished.
* Loads a set scene when the player collides with an attached gameobject.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneProgresser : DoorManager
{
    public string SceneName;

    public override void EnterDoor()
    {
        SceneManager.LoadScene(SceneName);
    }

    protected override void AttemptEnterDoor()
    {
        open = ThisRoom.CheckRoomCleared();

        if (open)
            EnterDoor();
    }
}
