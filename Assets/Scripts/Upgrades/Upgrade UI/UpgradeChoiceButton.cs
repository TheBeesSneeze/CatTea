/*******************************************************************************
* File Name :         UpgradeChoiceButton.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/7/2023
*
* Brief Description : 
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
        UpgradePrefab = GameManager.Instance.CurrentUpgradePool[upgradePrefabIndex];
        UpgradeIndex = upgradePrefabIndex;

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

        GameManager.Instance.CurrentUpgradePool.RemoveAt(UpgradeIndex);

        GameObject.FindObjectOfType<UpgradeChoiceInterface>().CloseUI();
    }
}
