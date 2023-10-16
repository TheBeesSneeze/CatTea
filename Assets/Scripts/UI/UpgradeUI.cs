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
    public List<Image> IconList;
    public void LoadNewUpgrade(UpgradeType newUpgrade)
    {
        Sprite upgradeIcon = newUpgrade.DisplaySprite;

        Image newIcon = IconList[NumberOfUpgrades];
        newIcon.gameObject.SetActive(true);
        newIcon.sprite = upgradeIcon;

        //change x position of ui icon based on how many things there are

        
        NumberOfUpgrades++;

    }
}
