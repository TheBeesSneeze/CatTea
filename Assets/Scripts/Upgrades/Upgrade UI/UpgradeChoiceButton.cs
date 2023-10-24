/*******************************************************************************
* File Name :         UpgradeChoiceButton.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/7/2023
*
* Brief Description : 1 of the 3 buttons on the upgrade choice menu. Loads info
* from the upgrade prefabs
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeChoiceButton : MonoBehaviour
{
    //public static Color SelectedColor;
    public UpgradeChoiceInterface upgradeUI;

    public TextMeshProUGUI Header;
    public TextMeshProUGUI Description;

    [Header("Debug")]
    public GameObject UpgradePrefab;
    public int UpgradeIndex;

    //private Color unselectedColor;
    //private SpriteRenderer spriteRenderer;

    private void Start()
    {
        upgradeUI = GameObject.FindObjectOfType<UpgradeChoiceInterface>();
    }

    public void LoadUpgrade(int upgradePrefabIndex, bool updateButtonText)
    {
        UpgradePrefab = GameManager.Instance.CurrentUpgradePool[upgradePrefabIndex];
        UpgradeIndex = upgradePrefabIndex;

        if (!updateButtonText)
            return;

        UpgradeType upgrade = UpgradePrefab.GetComponent<UpgradeType>();

        Header.text = upgrade.DisplayName;
        Description.text = upgrade.DisplayDescription;
    }

    
    public void OnClick()
    {
        upgradeUI.SelectOption(this);
    }

    /// <summary>
    /// Remove the upgrade from GameManagers pool
    /// </summary>
    public void ConfirmUpgrade()
    {
        Instantiate(UpgradePrefab);

        GameManager.Instance.CurrentUpgradePool.RemoveAt(UpgradeIndex);
    }
}
