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
using UnityEngine;

public class UpgradeType : MonoBehaviour
{
    //[Tooltip("Mark true if the effect should run at start. Good for one time use upgrades like stat upgrades.")]
    //public bool PerformEffectOnStart;

    public enum UpgradeActivationType { OnStart, OnEnemyDeath, OnEnemyDamage, OnPlayerDamage, OnEnterRoom, OnPlayerGun, OnPlayerSword, OnPlayerDash}

    [Tooltip("When this upgrades effect runs")]
    public UpgradeActivationType ActivationType;

    //unity stuff
    [HideInInspector]protected PlayerBehaviour playerBehaviour;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();

        if(ActivationType.Equals(UpgradeActivationType.OnStart))
        {
            UpgradeEffect();
            return;
            //this code lays dormant and does nothing for the rest of the game forever
        }

        AssignActivationEvent();
    }

    /// <summary>
    /// Effect that runs when upgrade trigger is activated.
    /// Ex: if upgrade should make an enemy bleed, UpgradeEffect only makes the enemy bleed.
    /// </summary>
    public virtual void UpgradeEffect()
    {
        Debug.LogWarning("Override me!");
    }

    /// <summary>
    /// UpgradeEffect base for when position matters
    /// </summary>
    /// <param name="eventPosition">Where the thing happened</param>
    public virtual void UpgradeEffect(Vector3 eventPosition)
    {
        Debug.LogWarning("Override me!");
    }

    private void AssignActivationEvent()
    {
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
                GameEvents.Instance.PlayerGunAction += UpgradeEffect;
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
