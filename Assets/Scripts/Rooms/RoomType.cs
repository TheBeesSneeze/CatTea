/*******************************************************************************
* File Name :         BossRoom.cs
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
    public bool RoomCleared;
    public bool RoomLoaded;
    public bool OpenDoorsOnStart;

    /// <summary>
    /// initializes everything in the room
    /// </summary>
    public virtual void EnterRoom()
    {

    }
}
