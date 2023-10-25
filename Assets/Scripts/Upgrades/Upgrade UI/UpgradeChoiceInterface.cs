/*******************************************************************************
* File Name :         UpgradeChoiceInterface.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/7/2023
*
* Brief Description : Gives upgrades to each of the buttons.
* Upgrades are chosen randomly so that there wont be duplicates. The mystery item
* is chosen before the user clicks on it.
* 
* TODO: cool transition?
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeChoiceInterface : MonoBehaviour
{
    public GameObject UpgradeScreen;

    [Header("Upgrade Buttons")]
    public UpgradeChoiceButton UpgradeButton1;
    public UpgradeChoiceButton UpgradeButton2;
    public UpgradeChoiceButton MysteryButton;

    [HideInInspector] public UpgradeChoiceObject ChoiceObject;

    private void Start()
    {
        UpgradeScreen.SetActive(false);
    }

    public void OpenUI()
    {
        UpgradeScreen.SetActive(true);

        RandomizeUpgradeChoices();
    }

    /// <summary>
    /// Call when upgrade interaction is done.
    /// Destroys ChoiceObject
    /// </summary>
    public void CloseUI()
    {
        Destroy(ChoiceObject.gameObject);
        UpgradeScreen.SetActive(false);
        
    }

    /// <summary>
    /// Gets random upgrade choices (by index) and sends it to buttons.
    /// </summary>
    private void RandomizeUpgradeChoices()
    {
        List<int> upgradeIndexes = Enumerable.Range(0, GameManager.Instance.CurrentUpgradePool.Count).ToList();

        if(upgradeIndexes.Count < 3)
        {
            Debug.LogWarning("Not enough upgrades");
            UpgradeScreen.SetActive(false);
            return;
        }

        UpgradeButton1.LoadUpgrade(GetRandomIndex(ref upgradeIndexes), true);
        UpgradeButton2.LoadUpgrade(GetRandomIndex(ref upgradeIndexes), true);
        MysteryButton.LoadUpgrade(GetRandomIndex(ref upgradeIndexes), false);
    }

    private int GetRandomIndex(ref List<int> upgradeIndexes)
    {
        int randomIndex = Random.Range(0, upgradeIndexes.Count);
        int result = upgradeIndexes[randomIndex];
        upgradeIndexes.RemoveAt(randomIndex);

        return result;
    }
}
