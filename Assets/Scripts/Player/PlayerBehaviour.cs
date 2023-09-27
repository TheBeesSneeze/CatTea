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

    [HideInInspector] public int PrimaryAttackDamage;
    [HideInInspector] public float PrimaryAttackSpeed;
    [HideInInspector] public float PrimaryAttackCoolDown;
    [HideInInspector] public float PrimaryAttackKnockback;

    [HideInInspector] public int SecondaryAttackDamage;
    [HideInInspector] public float SecondaryAttackSpeed;
    [HideInInspector] public float SecondaryAttackCoolDown;
    [HideInInspector] public float SecondaryAttackKnockback;

    //components
    private DefaultPlayerController playerController;
    public enum WeaponType { Default, Melee, Ranged };
    [HideInInspector] public WeaponType PlayerWeapon; //assigned automatically. Default by, well, default

    private PlayerHealthBar healthBar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<DefaultPlayerController>();

        try { healthBar = GameObject.FindObjectOfType<PlayerHealthBar>(); }
        catch { Debug.LogWarning("No Player Health Bar in Scene"); }
        
        SetStatsToDefaults();
        HealthPoints = MaxHealthPoints;
    }

    public override void SetHealth(int Value)
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
        bool died = base.TakeDamage(Damage);

        BecomeInvincible(InvincibilitySeconds);

        return died;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if(tag.Equals("Enemy Attack"))
        {
            AttackType attack = collision.GetComponent<AttackType>();

            if(attack != null)
            {
                TakeDamage(attack.Damage);
            }

            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }

    public override void SetStatsToDefaults()
    {
        Speed = CurrentPlayerStats.Speed;
        MaxHealthPoints = CurrentPlayerStats.MaxHealthPoints;
        InvincibilitySeconds = CurrentPlayerStats.InvincibilitySeconds;

        DashRechargeSeconds = CurrentPlayerStats.DashRechargeSeconds;
        DashUnits = CurrentPlayerStats.DashUnits;
        DashTime = CurrentPlayerStats.DashTime;
        
        PrimaryAttackDamage = CurrentPlayerStats.PrimaryAttackDamage;
        PrimaryAttackSpeed = CurrentPlayerStats.PrimaryAttackSpeed;
        PrimaryAttackCoolDown = CurrentPlayerStats.PrimaryAttackCoolDown;
        PrimaryAttackKnockback = CurrentPlayerStats.PrimaryAttackKnockback;

        SecondaryAttackDamage = CurrentPlayerStats.SecondaryAttackDamage;
        SecondaryAttackSpeed = CurrentPlayerStats.SecondaryAttackSpeed;
        SecondaryAttackCoolDown = CurrentPlayerStats.SecondaryAttackCoolDown;
        SecondaryAttackKnockback = CurrentPlayerStats.SecondaryAttackKnockback;
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