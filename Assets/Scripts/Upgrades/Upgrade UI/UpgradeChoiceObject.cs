/*******************************************************************************
* File Name :         UpgradeChoiceObject.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/7/2023
*
* Brief Description : The object that the user presses E at.
* prompts the user to select the upgrade.
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeChoiceObject : MonoBehaviour
{
    public GameObject ButtonPrompt;

    private bool playerInteracted = false;

    [SerializeField] private UpgradeChoiceInterface UpgradeUICanvas;

    private void Start()
    {
        UpgradeUICanvas = GameObject.FindObjectOfType<UpgradeChoiceInterface>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(true);

            PlayerController.Instance.Select.started += ActivateUpgradeUI;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(false);

            PlayerController.Instance.Select.started -= ActivateUpgradeUI;
            //PlayerController.Instance.IgnoreAllInputs = false;
        }
    }

    private void ActivateUpgradeUI(InputAction.CallbackContext obj)
    {
        PlayerController.Instance.Select.started -= ActivateUpgradeUI;

        UpgradeUICanvas.OpenUI(!playerInteracted);

        playerInteracted = true;

        UpgradeUICanvas.ChoiceObject = this;
    }
}
