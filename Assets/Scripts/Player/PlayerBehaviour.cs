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

    private void Awake()
    {
        SetStatsToDefaults();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();

        try { healthBar = GameObject.FindObjectOfType<PlayerHealthBar>(); }
        catch { Debug.LogWarning("No Player Health Bar in Scene"); }
        
        HealthPoints = MaxHealthPoints;
    }

    public override void SetHealth(float Value)
    {
        base.SetHealth(Value);

        if (healthBar != null) 
            healthBar.UpdateHealth();
        if(HealthPoints <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public override bool TakeDamage(int Damage)
    {
        GameEvents.Instance.OnPlayerDamage();

        bool died = base.TakeDamage(Damage);

        BecomeInvincible(InvincibilitySeconds);

        return died;
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
        ShotsPerBurst = CurrentPlayerStats.ShotsShotsPerBurst;
        TimeBetweenShots = CurrentPlayerStats.TimeBetweenShots;
        AmmoRechargeTime = CurrentPlayerStats.AmmoRechargeTime;
        RangedAttackKnockback = CurrentPlayerStats.RangedAttackKnockback;

        MeleeAttackDamage = CurrentPlayerStats.MeleeAttackDamage;
        SwingSeconds = CurrentPlayerStats.SwingSeconds;
        SwordAttackCoolDown = CurrentPlayerStats.SwordAttackCoolDown;
        MeleeAttackKnockback = CurrentPlayerStats.MeleeAttackKnockback;
    }
}


/*******************************************************************************
* Class Name :        PlayerData
* Author(s) :         Toby Schamberger
* Creation Date :     9/26/2023
*
* Brief Description : hell if i know man
*****************************************************************************/
public class PlayerData
{

}