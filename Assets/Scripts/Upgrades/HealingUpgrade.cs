/*******************************************************************************
* File Name :         HealthUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/30/2023
*
* Brief Description : Heals the player + n HP every time they enter a room
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingUpgrade : UpgradeType
{
    public float HealAmount;
    public override void UpgradeEffect()
    {
        PlayerBehaviour.Instance.HealthPoints = PlayerBehaviour.Instance.HealthPoints + HealAmount;
    }
}
