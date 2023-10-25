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

    [SerializeField] private UpgradeChoiceInterface UpgradeUICanvas;
    private PlayerController playerController;

    private void Start()
    {
        UpgradeUICanvas = GameObject.FindObjectOfType<UpgradeChoiceInterface>();
        playerController = GameObject.FindObjectOfType<PlayerBehaviour>().GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(true);

            playerController = collision.GetComponent<PlayerController>();
            playerController.Select.started += ActivateUpgradeUI;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(false);

            playerController.Select.started -= ActivateUpgradeUI;
            playerController.IgnoreAllInputs = false;
        }
    }

    private void ActivateUpgradeUI(InputAction.CallbackContext obj)
    {
        playerController.Select.started -= ActivateUpgradeUI;
        //if(UpgradeUICanvas == null)


        UpgradeUICanvas.OpenUI();

        UpgradeUICanvas.ChoiceObject = this;
    }
}
