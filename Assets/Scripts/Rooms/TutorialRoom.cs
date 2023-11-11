/*******************************************************************************
* File Name :         TutorialRoom.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/10/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : RoomType
{
    public override void ExitRoom()
    {
        SaveDataManager.Instance.SaveData.TutorialCompleted = true;
        SaveDataManager.Instance.SaveSaveData();

        base.ExitRoom();
    }
}
