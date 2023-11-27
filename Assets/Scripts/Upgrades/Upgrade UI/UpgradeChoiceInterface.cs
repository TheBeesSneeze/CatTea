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
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeChoiceInterface : MonoBehaviour
{
    private Color deselectedColor;

    public GameObject UpgradeScreen;

    [Header("Upgrade Buttons")]
    public UpgradeChoiceButton UpgradeButton1;
    public UpgradeChoiceButton UpgradeButton2;
    public UpgradeChoiceButton MysteryButton;

    public Button ConfirmUpgradeButton;

    [HideInInspector] public UpgradeChoiceObject ChoiceObject;

    private UpgradeChoiceButton selectedButton;

    private void Start()
    {
        UpgradeScreen.SetActive(false);

        deselectedColor = UpgradeButton1.GetComponent<Image>().color;
        UpgradeButton2.GetComponent<Image>().color = deselectedColor;
        MysteryButton.GetComponent<Image>().color = deselectedColor;
    }

    public void OpenUI(bool randomizeChoices)
    {
        PlayerController.Instance.IgnoreAllInputs = true;
        PlayerController.Instance.Pause.started += CancelUI;

        UpgradeScreen.SetActive(true);

        ConfirmUpgradeButton.interactable = false;
        DeselectAllOptions();

        if(randomizeChoices)
        {
            RandomizeUpgradeChoices();
        }
    }

    /// <summary>
    /// Cancels UI interface, player can come back tho
    /// </summary>
    /// <param name="obj"></param>
    public void CancelUI(InputAction.CallbackContext obj)
    {
        PlayerController.Instance.Pause.started -= CancelUI;
        PlayerController.Instance.IgnoreAllInputs = false;

        UpgradeScreen.SetActive(false);
    }

    /// <summary>
    /// Call when upgrade interaction is done.
    /// Destroys ChoiceObject
    /// </summary>
    public void CloseUI()
    {
        PlayerController.Instance.IgnoreAllInputs = false;

        Destroy(ChoiceObject.gameObject);
        UpgradeScreen.SetActive(false);
    }

    /// <summary>
    /// Called when the player presses the confirm button
    /// </summary>
    public void ConfirmSelection()
    {
        selectedButton.ConfirmUpgrade();
        CloseUI();
    }

    public void SelectOption(UpgradeChoiceButton choice)
    {
        DeselectAllOptions();

        selectedButton = choice;

        Image choiceSprite = choice.GetComponent<Image>();

        choiceSprite.color = choice.GetComponent<Button>().colors.selectedColor;

        ConfirmUpgradeButton.interactable = true;
    }

    public void DeselectAllOptions()
    {
        UpgradeButton1.GetComponent<Image>().color = deselectedColor;
        UpgradeButton2.GetComponent<Image>().color = deselectedColor;
        MysteryButton.GetComponent<Image>().color = deselectedColor;

        EventSystem.current.SetSelectedGameObject(null);

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
        /*
        bool validUpgrade = false;
        int result;

        do
        {
            int randomIndex = Random.Range(0, upgradeIndexes.Count);
            result = upgradeIndexes[randomIndex];

            validUpgrade = CheckIfUpgradeIsValid(result);
            if(validUpgrade)
            {
                Debug.Log(GameManager.Instance.CurrentUpgradePool[result]);
            }

            upgradeIndexes.RemoveAt(randomIndex);
        }
        while (!validUpgrade || !SaveDataManager.Instance.SaveData.GunUnlocked);

        return result;
        */
    }

    /// <summary>
    /// checks if upgrade can be collected (based on gun collected vs if upgrade recquires gun)
    /// </summary>
    /// <returns></returns>
    private bool CheckIfUpgradeIsValid(int index)
    {
        Debug.Log("dont use this fucking function ");
        GameObject upgrade = GameManager.Instance.CurrentUpgradePool[index];

        UpgradeType upgradeType = upgrade.GetComponent<UpgradeType>();

        return (!upgradeType.RecquiresGun || SaveDataManager.Instance.SaveData.GunUnlocked);
    }
}
