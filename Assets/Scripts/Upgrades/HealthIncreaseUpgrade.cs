/*******************************************************************************
* File Name :         HealthIncreaseUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/7/2023
*
* Brief Description : Increases the players health.
* Also heals them that much
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncreaseUpgrade : UpgradeType
{
    [Tooltip("How much to increase health by")]
    public float HealthIncrease;

    public override void UpgradeEffect()
    {
        PlayerBehaviour.Instance.MaxHealthPoints += HealthIncrease;

        PlayerBehaviour.Instance.HealthPoints += HealthIncrease;
    }
}
