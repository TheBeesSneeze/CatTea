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
    public TextMeshProUGUI Header;
    public TextMeshProUGUI Description;

    [Header("Debug")]
    public GameObject UpgradePrefab;
    public int UpgradeIndex;

    public void LoadUpgrade(int upgradePrefabIndex, bool updateButtonText)
    {
        UpgradeIndex = upgradePrefabIndex;
        UpgradePrefab = GameManager.Instance.CurrentUpgradePool[upgradePrefabIndex];
        //Debug.Log(UpgradeIndex);

        if (!updateButtonText)
            return;

        
        UpgradeType upgrade = UpgradePrefab.GetComponent<UpgradeType>();

        Header.text = upgrade.DisplayName;
        Description.text = upgrade.DisplayDescription;
    }

    /// <summary>
    /// Remove the upgrade from GameManagers pool
    /// </summary>
    public void OnClick()
    {
        Instantiate(UpgradePrefab);
        
        UpgradeType upgrade = UpgradePrefab.GetComponent<UpgradeType>();

        if(!upgrade.Stackable)
        {
            GameManager.Instance.CurrentUpgradePool.RemoveAt(UpgradeIndex);
        }

        UpgradeChoiceInterface upgradeUI = GameObject.FindObjectOfType<UpgradeChoiceInterface>();

        upgradeUI.CloseUI();
    }
}
