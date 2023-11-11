/*******************************************************************************
* File Name :         GunPickup.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/10/2023
*
* Brief Description : permenately unlocks the gun for the player when on collision.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public DoorManager BossDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if (!tag.Equals("Player"))
            return;

        SaveDataManager.Instance.SaveData.GunUnlocked = true;
        PlayerBehaviour.Instance.OnGunUnlocked();

        if(BossDoor != null )
            BossDoor.OpenDoor();

        Destroy(gameObject);
    }
}
