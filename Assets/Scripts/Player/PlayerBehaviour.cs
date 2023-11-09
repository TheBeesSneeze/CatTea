/*******************************************************************************
* File Name :         PlayerBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : The player class which deals specifically with everything 
* but player controls.
* Meant to avoid clutter
* 
* TODO:
* Collisions
* Health
* Upgrades???
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : CharacterBehaviour
{
    //Player Stats
    public PlayerStats CurrentPlayerStats;

    [HideInInspector] public float InvincibilitySeconds;

    [HideInInspector] public float DashRechargeSeconds;
    [HideInInspector] public float DashTime;
    [HideInInspector] public float DashUnits;

    [HideInInspector] public int RangedAttackDamage;
    [HideInInspector] public float ProjectileSpeed;
    [HideInInspector] public int ShotsPerBurst;
    [HideInInspector] public float TimeBetweenShots;
    [HideInInspector] public float AmmoRechargeTime;
    [HideInInspector] public float RangedAttackKnockback;

    [HideInInspector] public int MeleeAttackDamage;
    [HideInInspector] public float SwingSeconds;
    [HideInInspector] public float SwordAttackCoolDown;
    [HideInInspector] public float MeleeAttackKnockback;

    //components
    private PlayerController playerController;

    private PlayerHealthBar healthBar;
    private PlayerAmmoBar ammoBar;

    private void Awake()
    {
        SetStatsToDefaults();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();

        healthBar = GameObject.FindObjectOfType<PlayerHealthBar>();
        ammoBar = GameObject.FindObjectOfType<PlayerAmmoBar>();
        
        HealthPoints = MaxHealthPoints;

        StartSwordMode();

        if( ! SaveDataManager.Instance.SaveData.GunUnlocked)
        {
            OnGunLocked();
        }

    }

    /// <summary>
    /// Hides sword and brings out gun.
    /// Does not do any attacking.
    /// </summary>
    public void StartGunMode()
    {
        playerController.GunSprite.enabled = true;
        playerController.SwordSprite.enabled = false;
    }

    /// <summary>
    /// Hides gun and brings out sword.
    /// Does not do any attacking.
    /// </summary>
    public void StartSwordMode()
    {
        playerController.GunSprite.enabled = false;
        playerController.SwordSprite.enabled = true;
    }

    /// <summary>
    /// Hides ammo UI and aim icon
    /// </summary>
    private void OnGunLocked()
    {
        ammoBar.enabled = false;
    }

    public override void SetHealth(float Value)
    {
        base.SetHealth(Value);

        if (healthBar != null) 
            healthBar.UpdateHealth();
    }

    public override bool TakeDamage(float Damage)
    {
        return TakeDamage(Damage, true);
    }

    public override bool TakeDamage(float damage, bool onDamageEvent)
    {
        if (onDamageEvent)
            GameEvents.Instance.OnPlayerDamage();

        bool died = base.TakeDamage(damage);

        BecomeInvincible(InvincibilitySeconds); 

        return died;
    }

    public override void Die()
    {
        SceneManager.LoadScene(3);
    }

    public override void SetStatsToDefaults()
    {
        Speed = CurrentPlayerStats.Speed;
        MaxHealthPoints = CurrentPlayerStats.MaxHealthPoints;
        InvincibilitySeconds = CurrentPlayerStats.InvincibilitySeconds;

        DashRechargeSeconds = CurrentPlayerStats.DashRechargeSeconds;
        DashUnits = CurrentPlayerStats.DashUnits;
        DashTime = CurrentPlayerStats.DashTime;
        
        RangedAttackDamage = CurrentPlayerStats.RangedAttackDamage;
        ProjectileSpeed = CurrentPlayerStats.ProjectileSpeed;
        ShotsPerBurst = CurrentPlayerStats.ShotsPerBurst;
        TimeBetweenShots = CurrentPlayerStats.TimeBetweenShots;
        AmmoRechargeTime = CurrentPlayerStats.AmmoRechargeTime;
        RangedAttackKnockback = CurrentPlayerStats.RangedAttackKnockback;

        MeleeAttackDamage = CurrentPlayerStats.MeleeAttackDamage;
        SwingSeconds = CurrentPlayerStats.SwingSeconds;
        SwordAttackCoolDown = CurrentPlayerStats.SwordAttackCoolDown;
        MeleeAttackKnockback = CurrentPlayerStats.MeleeAttackKnockback;
    }
}
