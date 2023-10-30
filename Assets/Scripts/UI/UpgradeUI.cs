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
        Image newIcon = IconList[NumberOfUpgrades];
        UpgradeIcon icon = newIcon.GetComponent<UpgradeIcon>();
        icon.LoadUpgrade(newUpgrade);

        //change x position of ui icon based on how many things there are

        NumberOfUpgrades++;
    }
}
