/*******************************************************************************
* File Name :         DestroyOtherProjectilesUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/16/2023
*
* Brief Description : Runs on OnPlayerShoot. also does shit in start.
* Does what the name says it does
* TODO: make sword parry projectiles
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//long ass name 
public class DestroyOtherProjectilesUpgrade : UpgradeType
{
    protected override void Start()
    {
        base.Start();

        GameObject.Find("Sword").AddComponent<ParryProjectiles>();
    }
    public override void UpgradeEffect()
    {
        //AttackType playerBullet = rangedPlayerController.BulletPrefab.GetComponent<AttackType>();
        //playerBullet.DestroyOtherAttacks = true;

        GameObject[] allPlayerAttacks = GameObject.FindGameObjectsWithTag("Player Attack");

        foreach (GameObject attackGameObject in allPlayerAttacks) 
        {
            AttackType attack = attackGameObject.GetComponent<AttackType>();

            if (attack != null)
            {
                attack.DestroyOtherAttacks = true;
                attack.GetDestroyedByOtherAttacks = false;
            }
        }
    }
}
