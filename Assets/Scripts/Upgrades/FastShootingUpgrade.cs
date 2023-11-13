/*******************************************************************************
* File Name :         FastShootingUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/13/2023
*
* Brief Description : Decreases time between shots and decreases ammo recharge time
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastShootingUpgrade : UpgradeType
{
    public float RechargeTimeMultiplier;
    public float ProjectileSpeedMultiplier;
    public float TimeBetweenShotsMultiplier;
    public override void UpgradeEffect()
    {
        PlayerBehaviour.Instance.AmmoRechargeTime = PlayerBehaviour.Instance.AmmoRechargeTime * RechargeTimeMultiplier;
        PlayerBehaviour.Instance.ProjectileSpeed = PlayerBehaviour.Instance.ProjectileSpeed * ProjectileSpeedMultiplier;
        PlayerBehaviour.Instance.TimeBetweenShots = PlayerBehaviour.Instance.TimeBetweenShots * TimeBetweenShotsMultiplier;
    }
}
