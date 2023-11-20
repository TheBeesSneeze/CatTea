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
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : CharacterBehaviour
{
    public static PlayerBehaviour Instance;

    public enum Weapon { Sword, Gun};
    public Weapon WeaponSelected;

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
    public float AmmoRechargeTime;
    [HideInInspector] public float RangedAttackKnockback;

    [HideInInspector] public int MeleeAttackDamage;
    [HideInInspector] public float SwingSeconds;
    [HideInInspector] public float SwordAttackCoolDown;
    [HideInInspector] public float MeleeAttackKnockback;

    //magic numbers
    private Color damageScreenColor = new Color(1, 0.7f, 0.7f); //the reddest the screen can be
    private float secondsUntilDeathScreen = 3f;

    //components
    private PlayerController playerController;
    private RangedPlayerController rangedPlayerController;

    private PlayerHealthBar healthBar;
    private PlayerAmmoBar ammoBar;
    private Light2D globalLight;

    protected override void Awake()
    {
        SetStatsToDefaults();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
            Instance = this;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();
        rangedPlayerController = GetComponent<RangedPlayerController>();

        healthBar = GameObject.FindObjectOfType<PlayerHealthBar>();
        ammoBar = GameObject.FindObjectOfType<PlayerAmmoBar>();

        globalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();

        HealthPoints = MaxHealthPoints;

        StartSwordMode();

        SaveDataManager.Instance.LoadSaveData();

        if (SaveDataManager.Instance.SaveData.GunUnlocked)
        {
            OnGunUnlocked(); 
        }
        else
            OnGunLocked();

    }

    /// <summary>
    /// Hides sword and brings out gun.
    /// Does not do any attacking.
    /// </summary>
    public void StartGunMode()
    {
        playerController.GunSprite.enabled = true;
        playerController.SwordSprite.enabled = false;

        WeaponSelected = Weapon.Gun;
    }

    /// <summary>
    /// Hides gun and brings out sword.
    /// Does not do any attacking.
    /// </summary>
    public void StartSwordMode()
    {
        playerController.GunSprite.enabled = false;
        playerController.SwordSprite.enabled = true;

        WeaponSelected = Weapon.Sword;
    }

    /// <summary>
    /// Hides ammo UI and aim icon
    /// </summary>
    public void OnGunLocked()
    {
        ammoBar.gameObject.SetActive(false);
        rangedPlayerController.RangedIcon.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnGunUnlocked()
    {
        ammoBar.gameObject.SetActive(true);
        rangedPlayerController.RangedIcon.GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void SetHealth(float Value)
    {
        base.SetHealth(Value);

        if (healthBar != null) 
            healthBar.UpdateHealth();

        UpdateScreenColor();
    }

    /// <summary>
    /// updates screen color by health
    /// </summary>
    public void UpdateScreenColor()
    {
        float t = 1 - (HealthPoints / MaxHealthPoints);
        float tScaled = Mathf.Pow(t, 7);

        globalLight.color = Color.Lerp(Color.white, damageScreenColor, tScaled);

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
        playerController.IgnoreAllInputs = true;

        SaveDataManager.Instance.SaveData.RunNumber++;
        SaveDataManager.Instance.SaveSaveData();

        StartCoroutine(DarkenScreen());

        
    }

    /// <summary>
    /// Runs on death. darkens screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator DarkenScreen()
    {
        float old = globalLight.intensity;

        float t = 0;
        while (t<secondsUntilDeathScreen) 
        {
            t += Time.deltaTime;

            globalLight.intensity = Mathf.Lerp(old, 0, t / secondsUntilDeathScreen);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

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
