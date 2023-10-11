/*******************************************************************************
* File Name :         RangedPlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Override of DefaultPlayerController for the ranged weapons controls.
* 
* TODO: 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedPlayerController : DefaultPlayerController
{
<<<<<<< Updated upstream
    protected override void Start()
=======
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
    public AudioSource ShootSound;


    public int BulletsLeft
>>>>>>> Stashed changes
    {
        base.Start();
    }

    protected override void Primary_performed(InputAction.CallbackContext obj)
    {
<<<<<<< Updated upstream
        //TODO
=======
        playerBehaviour = GetComponent<PlayerBehaviour>();
        playerController = GetComponent<PlayerController>();
        meleePlayerController = GetComponent<MeleePlayerController>();

        AmmoBar = GameObject.FindObjectOfType<PlayerAmmoBar>();

        RangedIcon.transform.SetParent(null);

        canShoot = true;

        RechargeAmmo();

        
>>>>>>> Stashed changes
    }
    protected override void Primary_canceled(InputAction.CallbackContext obj)
    {
        //TODO
    }
    protected override void Secondary_performed(InputAction.CallbackContext obj)
    {
        //TODO
    }
    protected override void Secondary_canceled(InputAction.CallbackContext obj)
    {
<<<<<<< Updated upstream
        //TODO
=======
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
        ShootSound.Play();
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
>>>>>>> Stashed changes
    }
}
