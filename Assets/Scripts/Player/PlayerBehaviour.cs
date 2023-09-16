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

public class PlayerBehaviour : CharacterBehaviour
{
    //Player Stats
    public PlayerStats CurrentPlayerStats;
    [Header("Derived from PlayerStats, do not tweak in editor")]
    public float DashRechargeSeconds;
    public float DashForce;

    public float PrimaryAttackDamage;
    public float PrimaryAttackSpeed;
    public float PrimaryAttackCoolDown;

    public float SecondaryAttackDamage;
    public float SecondaryAttackSpeed;
    public float SecondaryAttackCoolDown;

    //components
    private DefaultPlayerController playerController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<DefaultPlayerController>();
        SetStatsToDefaults();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        Debug.Log(tag);

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
        HealthPoints = CurrentPlayerStats.HealthPoints;
        DashRechargeSeconds = CurrentPlayerStats.DashRechargeSeconds;
        DashForce = CurrentPlayerStats.DashForce;
        
        PrimaryAttackDamage = CurrentPlayerStats.PrimaryAttackDamage;
        PrimaryAttackSpeed = CurrentPlayerStats.PrimaryAttackSpeed;
        PrimaryAttackCoolDown = CurrentPlayerStats.PrimaryAttackCoolDown;

        SecondaryAttackDamage = CurrentPlayerStats.SecondaryAttackSpeed;
        SecondaryAttackSpeed = CurrentPlayerStats.SecondaryAttackDamage;
        SecondaryAttackCoolDown = CurrentPlayerStats.SecondaryAttackCoolDown;
    }
}
