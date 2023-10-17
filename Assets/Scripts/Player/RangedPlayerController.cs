/*******************************************************************************
* File Name :         RangedPlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Override of playerController for the ranged weapons controls.
* 
* TODO: mouse controls??? please
* aim with right stick
* 
* dashing interrupts shooting
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedPlayerController : MonoBehaviour
{
    [Header("Ranged Settings")]
    public GameObject BulletPrefab;
    [Tooltip("Divides the players speed while they're shooting by this #")]
    public float SlowWhileShootingAmount;

    [Header("Unity stuff")]
    public Transform MirrorPivot;
    public Transform RotationPivot;
    public GameObject RangedIcon;
    public GameObject Gun;
    private PlayerAmmoBar AmmoBar;
    
    //le sound
    public AudioSource shootSound;

    public int BulletsLeft
    {
        get { return _bulletsLeft; }
        set { SetBulletsLeft(value); }
    }

    //really boring settings
    [HideInInspector] public float MaxUpAngle = 25f;
    [HideInInspector] public float MaxDownAngle = -25;

    //etc
    private PlayerBehaviour playerBehaviour;
    protected PlayerController playerController;
    protected MeleePlayerController meleePlayerController;

    private bool canShoot;

    [HideInInspector] public bool PrimaryShooting;
    private Coroutine shootingCoroutine;
    private Coroutine reloadAmmoCoroutine;

    public int _bulletsLeft;

    protected void Start()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
        playerController = GetComponent<PlayerController>();
        meleePlayerController = GetComponent<MeleePlayerController>();

        AmmoBar = GameObject.FindObjectOfType<PlayerAmmoBar>();

        RangedIcon.transform.SetParent(null);

        canShoot = true;

        RechargeAmmo();
    }

    public void Gun_performed(InputAction.CallbackContext obj)
    {
        playerController.StartGunMode();

        if (playerController.IgnoreAllInputs) return;

        if (PrimaryShooting || !canShoot)
            return;

        if (shootingCoroutine == null)
        {
            PrimaryShooting = true;
            shootingCoroutine = StartCoroutine(RepeatedFire());
        }
    }
    public void Gun_canceled(InputAction.CallbackContext obj)
    {
        PrimaryShooting = false;

        if (playerController.IgnoreAllInputs) return;

        //shootingCoroutine = null;
    }

    /// <summary>
    /// Repeatedly shoots.
    /// slows the player while shooting
    /// </summary>
    /// <returns></returns>
    private IEnumerator RepeatedFire()
    {
        float oldSpeed = playerBehaviour.Speed;
        playerBehaviour.Speed = oldSpeed / 2;

        while (PrimaryShooting && canShoot && BulletsLeft > 0)
        {
            BulletsLeft--;
            ShootBullet();
            yield return new WaitForSeconds(playerBehaviour.TimeBetweenShots);
        }

        playerBehaviour.Speed = oldSpeed;

        canShoot = true;
        RechargeAmmo();
        shootingCoroutine = null;
    }

    /// <summary>
    /// Mirrors the scale awesome style
    /// </summary>
    /// <returns></returns>
    public void CorrectGunPosition()
    {
        if (playerController.AimingDirection.x > 0)
            MirrorPivot.localScale = new Vector3(1, 1, 1);

        if (playerController.AimingDirection.x < 0)
            MirrorPivot.localScale = new Vector3(-1, 1, 1);
    }
    

    /// <summary>
    /// instantiates a bullet!
    /// this is gonna be bonkers once upgrades are added, i worry
    /// </summary>
    private void ShootBullet()
    {
        GameEvents.Instance.OnPlayerShoot();

        //spawn the thing
        GameObject bullet = Instantiate(BulletPrefab, Gun.transform.position, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        bulletRigidbody.velocity = playerController.AimingDirection * playerBehaviour.ProjectileSpeed;

        //rotate
        float Angle = Vector2.SignedAngle(Vector2.right, playerController.AimingDirection);
        Vector3 TargetRotation = new Vector3(0, 0, Angle);
        bullet.transform.eulerAngles = TargetRotation;

        shootSound.Play();
        
    }

    private void SetBulletsLeft(int value)
    {
        _bulletsLeft = value;

        if (AmmoBar == null)
            return;

        AmmoBar.UpdateAmmo();
    }

    /// <summary>
    /// (attempts to) start coroutine which reloads ammo over some interval
    /// </summary>
    public void RechargeAmmo()
    {
        if (reloadAmmoCoroutine != null)
            return;

        reloadAmmoCoroutine = StartCoroutine(RechargeAmmoCoroutine());
    }

    private IEnumerator RechargeAmmoCoroutine()
    {
        yield return new WaitForSeconds(playerBehaviour.AmmoRechargeTime);

        while (!PrimaryShooting && BulletsLeft < playerBehaviour.ShotsPerBurst)
        {
            BulletsLeft++;
            yield return new WaitForSeconds(playerBehaviour.AmmoRechargeTime);
        }
        reloadAmmoCoroutine = null;
    }
}
