/*******************************************************************************
* File Name :         SaveDataClass.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/6/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SaveDataClass 
{
    public int RunNumber=0;

    public bool TutorialCompleted=false;
    public bool GunUnlocked=false;

    public SaveDataClass (int runNumber, bool tutorialCompleted, bool gunUnlocked)
    {
        RunNumber = runNumber;
        TutorialCompleted = tutorialCompleted;
        GunUnlocked = gunUnlocked;
    }
}
