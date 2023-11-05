/*******************************************************************************
* File Name :         SettingsDataClass.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/4/2023
*
* Brief Description : stores volume and controller vibration and others maybe
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsDataClass
{
    public float SoundVolume; // 0 < x < 1
    public float MusicVolume; // "   "   "

    public bool ControllerVibration;
}

