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
using UnityEngine.InputSystem;

public class GunPickup : MonoBehaviour
{
    public SpriteRenderer ButtonPrompt;
    public GameObject GunTutorialPrefab;

    [HideInInspector] public DoorManager BossDoor;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        string tag = collider.tag;

        if (!tag.Equals("Player"))
            return;

        if (ButtonPrompt != null)
        {
            ButtonPrompt.enabled = true;
            Debug.Log("in");
        }

        PlayerController.Instance.Select.started += Interact;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        string tag = collider.tag;

        if (!tag.Equals("Player"))
            return;

        if (ButtonPrompt != null)
        {
            ButtonPrompt.enabled = false;
            Debug.Log("out");
        }

        PlayerController.Instance.Select.started -= Interact;
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        UnlockGun();
    }

    private void UnlockGun()
    {
        Instantiate(GunTutorialPrefab,null);

        SaveDataManager.Instance.SaveData.GunUnlocked = true;
        SaveDataManager.Instance.SaveSaveData();
        PlayerBehaviour.Instance.OnGunUnlocked();

        if (BossDoor != null)
        {
            BossDoor.ThisRoom.ForceCloseDoorOverride = false;
            BossDoor.OpenDoor();

        }
        Destroy(gameObject);
    }
}
