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
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : CharacterBehaviour
{
    //Player Stats
    public PlayerStats CurrentPlayerStats;
    [HideInInspector] public float DashRechargeSeconds;
    [HideInInspector] public float DashForce;

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

    private PlayerHealthBar healthBar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<DefaultPlayerController>();

        try { healthBar = GameObject.FindObjectOfType<PlayerHealthBar>(); }
        catch { Debug.LogWarning("No Player Health Bar in Scene"); }
        
        SetStatsToDefaults();
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
        HealthPoints = CurrentPlayerStats.HealthPoints;
        DashRechargeSeconds = CurrentPlayerStats.DashRechargeSeconds;
        DashForce = CurrentPlayerStats.DashForce;
        
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
