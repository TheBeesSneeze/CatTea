/*******************************************************************************
* File Name :         UpgradeIcon.cs
* Author(s) :         Toby Schamberger, 
* Creation Date :     10/29/2023
*
* Brief Description :
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeIcon : MonoBehaviour, IPointerEnterHandler
{
    public GameObject TextPanel;

    public TextMeshProUGUI UpgradeHeader;
    public TextMeshProUGUI UpgradeDescription;
    public Image UpgradeSprite;

    [Header("Debug")]
    public UpgradeType Upgrade;
    //[HideInInspector] private Sprite Icon;

    public void LoadUpgrade(UpgradeType upgrade)
    {
        this.gameObject.SetActive(true);
        TextPanel.SetActive(false);

        UpgradeSprite.sprite = upgrade.DisplaySprite;
        UpgradeHeader.text = upgrade.DisplayName;
        UpgradeDescription.text = upgrade.DisplayDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextPanel.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("fuck you");
        TextPanel.SetActive(false);
    }

}
