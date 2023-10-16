/*******************************************************************************
* File Name :         UpgradeUI.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/16/2023
*
* "Brief" Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public int NumberOfUpgrades = 0;
    public void LoadNewUpgrade(UpgradeType newUpgrade)
    {
        Image upgradeIcon = newUpgrade.DisplaySprite;

        //make a prefab for image icon ui thing

        //instantiate that guy

        //swap out the sprite

        //change x position of ui icon based on how many things there are

        NumberOfUpgrades++;
    }
}
