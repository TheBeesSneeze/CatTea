/*******************************************************************************
* File Name :         UpgradeChoiceObject.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/7/2023
*
* Brief Description : prompts the user to select the upgrade
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeChoiceObject : MonoBehaviour
{
    public GameObject ButtonPrompt;

    private UpgradeChoiceInterface UpgradeUICanvas;
    private PlayerController playerBehaviour;

    private void Start()
    {
        UpgradeUICanvas = GameObject.FindObjectOfType<UpgradeChoiceInterface>();
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>().GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(true);

            playerBehaviour = collision.GetComponent<PlayerController>();
            playerBehaviour.Select.started += ActivateUpgradeUI;
        }
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(false);

            playerBehaviour.Select.started -= ActivateUpgradeUI;
        }
    }

    private void ActivateUpgradeUI(InputAction.CallbackContext obj)
    {
        playerBehaviour.Select.started -= ActivateUpgradeUI;
        UpgradeUICanvas.OpenUI();
    }
}
