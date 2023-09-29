/*******************************************************************************
* File Name :         RangedPlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Override of DefaultPlayerController for the ranged weapons controls.
* 
* TODO: mouse controls??? please
* aim with right stick
* 
* dashing interrupts shooting
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedPlayerController : DefaultPlayerController
{
    [Header("Ranged Settings")]
    public GameObject BulletPrefab;
    [Tooltip("Divides the players speed while they're shooting by this #")]
    public float SlowWhileShootingAmount;

    [Header("Stats")] //ig
    public int ShotsPerBurst;
    public float ShootingCoolDownSeconds;

    [Header("Unity stuff")]
    public Transform MirrorPivot;
    public Transform RotationPivot;
    public GameObject RangedIcon;
    public GameObject Gun;

    //really boring settings
    private float MaxUpAngle = 25f;
    private float MaxDownAngle = -25;

    //etc
    private bool canShoot;

    private Vector3 shootingDirection;
    private bool primaryShooting;
    private Coroutine shootingCoroutine;
    private Coroutine endShootingCoroutine;
    private bool readShootingDirection;
    private Coroutine aimingCoroutine;

    protected override void Start()
    {
        base.Start();

        RangedIcon.transform.SetParent(null);

        playerBehaviour.PlayerWeapon = PlayerBehaviour.WeaponType.Ranged;

        UpdateShootingDirection();

        canShoot = true;
    }

    protected override void Primary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        if (primaryShooting)
            return;

        if (shootingCoroutine == null)
        {
            primaryShooting = true;
            shootingCoroutine = StartCoroutine(RepeatedFire());
        }
    }
    protected override void Primary_canceled(InputAction.CallbackContext obj)
    {
        primaryShooting = false;

        if (IgnoreAllInputs) return;

        //shootingCoroutine = null;
    }
    protected override void Secondary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;
        //TODO
    }
    protected override void Secondary_canceled(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;
        //TODO
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

        int bulletsShot=0;

        while (primaryShooting && canShoot && bulletsShot < ShotsPerBurst)
        {
            bulletsShot++;
            ShootBullet();
            yield return new WaitForSeconds(playerBehaviour.PrimaryAttackCoolDown);
        }

        playerBehaviour.Speed = oldSpeed;

        float scaledCooldownSeconds = (bulletsShot / ShotsPerBurst) * ShootingCoolDownSeconds;
        endShootingCoroutine = StartCoroutine(StopShooting(scaledCooldownSeconds));

        shootingCoroutine = null;
    }

    private IEnumerator StopShooting(float coolDownSeconds)
    {
        canShoot = false;

        yield return new WaitForSeconds(coolDownSeconds);

        canShoot = true;

        endShootingCoroutine = null;
    }

    /// <summary>
    /// Kind of unnessecary rn, but im cooking, ok
    /// </summary>
    public void UpdateShootingDirection()
    {
        if (aimingCoroutine != null)
            StopCoroutine(aimingCoroutine);

        readShootingDirection = true;

        if (PlayerControllerType.Equals(ControllerType.Keyboard))
        {
            RangedIcon.SetActive(true);
            aimingCoroutine = StartCoroutine(UpdateShootingDirectionByKeyboard());
        }

        if (PlayerControllerType.Equals(ControllerType.Controller))
        {
            RangedIcon.SetActive(false);
            aimingCoroutine = StartCoroutine(UpdateShootingDirectionByController());
        }
    }

    private IEnumerator UpdateShootingDirectionByKeyboard()
    {
        while(readShootingDirection)
        {
            Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RangedIcon.transform.position = MousePosition;

            //update shootingDirection. normalize MousePosition to be relative to player
            MousePosition -= (Vector2)transform.position;
            MousePosition = MousePosition.normalized;

            shootingDirection = MousePosition;

            //rotate awesome style
            MousePosition = new Vector2(Mathf.Abs(MousePosition.x), MousePosition.y);
            float angle = Mathf.Atan2(MousePosition.y, MousePosition.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, MaxDownAngle, MaxUpAngle);
            RotationPivot.transform.localEulerAngles = new Vector3(0,0,angle);

            CorrectGunPosition();

            yield return null;
        }
        aimingCoroutine = null;
    }

    private IEnumerator UpdateShootingDirectionByController()
    {
        while(readShootingDirection)
        {
            shootingDirection = inputDirection;

            CorrectGunPosition();

            yield return new WaitForEndOfFrame();
        }
        aimingCoroutine = null;
    }

    /// <summary>
    /// Mirrors the scale awesome style
    /// </summary>
    /// <returns></returns>
    private void CorrectGunPosition()
    {
        if(primaryShooting)
        {
            if (shootingDirection.x > 0)
                MirrorPivot.localScale = new Vector3(1, 1, 1);

            if (shootingDirection.x < 0)
                MirrorPivot.localScale = new Vector3(-1, 1, 1);

            return;
        }

        //else
        if (inputDirection.x > 0)
            MirrorPivot.localScale = new Vector3(1, 1, 1);

        if (inputDirection.x < 0)
            MirrorPivot.localScale = new Vector3(-1, 1, 1);
    }

    /// <summary>
    /// Makes the player face towards shootingDirection while shooting
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UpdateAnimation()
    {
        while (true)
        {
            if(primaryShooting)
            {
                myAnimator.SetFloat("XMovement", shootingDirection.x);
                myAnimator.SetFloat("YMovement", shootingDirection.y);
            }
            else
            {
                myAnimator.SetFloat("XMovement", moveDirection.x);
                myAnimator.SetFloat("YMovement", moveDirection.y);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    

    /// <summary>
    /// instantiates a bullet!
    /// this is gonna be bonkers once upgrades are added, i worry
    /// </summary>
    private void ShootBullet()
    {
        //spawn the thing
        GameObject bullet = Instantiate(BulletPrefab, Gun.transform.position, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        //fix the thing
        bulletRigidbody.velocity = shootingDirection * playerBehaviour.PrimaryAttackSpeed;
        float Angle = Vector2.SignedAngle(Vector2.right, shootingDirection);

        Vector3 TargetRotation = new Vector3(0, 0, Angle);
        bullet.transform.eulerAngles = TargetRotation;
    }
}
