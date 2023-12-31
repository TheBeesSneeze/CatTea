/*******************************************************************************
* File Name :         FirstBoss.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/10/2023
*
* Brief Description : Exactly the same as boss behaviour except it drops a gun
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : BossBehaviour
{
    public GameObject GunPickupPrefab;
    public override void Die()
    {
        MyRoom.ForceCloseDoorOverride = true;

        base.Die();
    }

    public override void DieForReal()
    {
        if( !SaveDataManager.Instance.SaveData.GunUnlocked)
        {
            DropUpgradeOnDeath = false;
        }

        AttemptToDropGun();
        base.DieForReal();
    }

    private void AttemptToDropGun()
    {
        if (SaveDataManager.Instance.SaveData.GunUnlocked)
        {
            MyRoom.ForceCloseDoorOverride = false;

            return;
        }

        GameObject gun = Instantiate(GunPickupPrefab, transform.position, GunPickupPrefab.transform.rotation);
        GunPickup gunPickup = gun.GetComponent<GunPickup>();

        gunPickup.BossDoor = MyRoom.Door;
    }
}
