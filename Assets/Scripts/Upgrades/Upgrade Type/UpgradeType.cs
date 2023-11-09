/*******************************************************************************
* File Name :         UpgradeType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/4/2023
*
* "Brief" Description : Base class for upgrades. Listens for certain events.
* 
* UpgradeEffect() is not called from external scripts directly. 
* GameEvents.cs listens for different flags to be triggered and calls UpgradeEffect();
* 
* ignore the blurb above if the activationType is OnStart. That does what it
* sounds like.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeType : MonoBehaviour
{
    public Sprite DisplaySprite;
    public string DisplayName;
    public string DisplayDescription;
    [Tooltip("Description for when the player already has this upgrade")]
    public string DuplicateDescription;
    public bool Stackable;

    public enum UpgradeActivationType {OnStart, OnEnemyDeath, OnEnemyDamage, OnPlayerDamage, OnEnterRoom, OnPlayerGun, OnPlayerSword, OnPlayerDash}

    [Tooltip("When this upgrades effect runs")]
    public UpgradeActivationType ActivationType;

    //unity stuff
    [HideInInspector] protected PlayerBehaviour playerBehaviour;
    [HideInInspector] protected RangedPlayerController rangedPlayerController;
    private UpgradeUI upgradeUI;

    [Header("Debug")]

    [Tooltip("How many times this upgrade has been collected")]
    public int NumberOfUpgrades;
    public bool IsADuplicate;

    protected UpgradeType originalUpgrade;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        rangedPlayerController = GameObject.FindObjectOfType<RangedPlayerController>();
        upgradeUI = GameObject.FindObjectOfType<UpgradeUI>();

        AddSpriteToUI();

        IsADuplicate = CheckIfDuplicate();

        if(IsADuplicate)
        {
            DuplicateUpgradeEffect();
            FindOriginalUpgrade();
            originalUpgrade.NumberOfUpgrades++;
            return;
        }

        NumberOfUpgrades = 1;

        if (ActivationType.Equals(UpgradeActivationType.OnStart))
        {
            UpgradeEffect();
            return;
            //this code lays dormant and does nothing for the rest of the game forever
        }

        AssignActivationEvent();
    }

    public virtual void DuplicateUpgradeEffect()
    {
        Debug.LogWarning("No override for duplicate upgrade effect in " + gameObject.name);
    }

    private bool CheckIfDuplicate()
    {
        int numberOfThis=0;
        UpgradeType[] upgrades = GameObject.FindObjectsOfType<UpgradeType>();

        for(int i=0; i< upgrades.Length; i++)
        {
            if (upgrades[i].gameObject.name == this.gameObject.name)
                numberOfThis++;

            if (numberOfThis >= 2)
                return true;
        }

        return false;
    }

    private void FindOriginalUpgrade()
    {
        UpgradeType[] upgrades = GameObject.FindObjectsOfType<UpgradeType>();

        foreach (UpgradeType upgrade in upgrades)
        {
            if(upgrade == this)
            {
                Debug.Log("same guy");
            }

            if (!upgrade.IsADuplicate)
            {
                originalUpgrade = upgrade;
                return;
            }
        }
    }

    private void AddSpriteToUI()
    {
        if(upgradeUI == null) 
        {
            Debug.LogWarning("No upgrade UI box present");
            return;
        }
        upgradeUI.LoadNewUpgrade(this);
    }

    /// <summary>
    /// Effect that runs when upgrade trigger is activated.
    /// Ex: if upgrade should make an enemy bleed, UpgradeEffect only makes the enemy bleed.
    /// </summary>
    public virtual void UpgradeEffect()
    {
        Debug.LogWarning("Override this upgrade!");
    }

    /// <summary>
    /// UpgradeEffect base for when position matters
    /// </summary>
    /// <param name="eventPosition">Where the thing happened</param>
    public virtual void UpgradeEffect(Vector3 eventPosition)
    {
        Debug.LogWarning("Override this upgrade!");
    }

    /// <summary>
    /// UpgradeEffect base for when character (boss, enemy, or player even (dont do player like this))
    /// </summary>
    /// <param name="characterEvent"></param>
    public virtual void UpgradeEffect(CharacterBehaviour eventCharacter)
    {
        Debug.LogWarning("Override this upgrade!");
    }

    /// <summary>
    /// UpgradeEffect base for bullets
    /// </summary>
    public virtual void UpgradeEffect(AttackType bullet)
    {
        Debug.LogWarning("Override this upgrade!");
    }

    private void AssignActivationEvent()
    {
        if (IsADuplicate)
            return;

        switch (ActivationType) 
        {
            case UpgradeActivationType.OnEnemyDeath:
                GameEvents.Instance.EnemyDeathAction += UpgradeEffect;
                break;
            case UpgradeActivationType.OnEnemyDamage:
                GameEvents.Instance.EnemyDamageAction += UpgradeEffect;
                break;
            case UpgradeActivationType.OnPlayerDamage:
                GameEvents.Instance.PlayerDamageAction += UpgradeEffect;
                break;
            case UpgradeActivationType.OnEnterRoom:
                GameEvents.Instance.RoomEnterAction += UpgradeEffect;
                break;
            case UpgradeActivationType.OnPlayerGun:
                GameEvents.Instance.PlayerShootAction += UpgradeEffect;
                break;
            case UpgradeActivationType.OnPlayerSword:
                GameEvents.Instance.PlayerSwordAction += UpgradeEffect;
                break;
            case UpgradeActivationType.OnPlayerDash:
                GameEvents.Instance.PlayerDashAction += UpgradeEffect;
                break;
        }
    }

    private void OnDestroy()
    {
        
    }
}
