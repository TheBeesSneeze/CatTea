/*******************************************************************************
* File Name :         RangedPlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Override of DefaultPlayerController for the ranged weapons controls.
* 
* TODO: mouse controls??? please
* aim with right stick
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedPlayerController : DefaultPlayerController
{
    [Header("Ranged Settings")]
    public GameObject BulletPrefab;
    public float BulletVelocity;
    public float SecondsBetweenShots;

    [Header("Unity stuff")]
    public Transform MirrorPivot;
    public GameObject Gun;

    //etc
    private Vector3 shootingDirection;
    private bool primaryShooting;
    private Coroutine shootingCoroutine;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CorrectGunPosition());
    }

    protected override void Primary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        if (primaryShooting)
            return;

        primaryShooting = true;
        shootingCoroutine = StartCoroutine(RepeatedFire());
    }
    protected override void Primary_canceled(InputAction.CallbackContext obj)
    {
        primaryShooting = false;

        if (IgnoreAllInputs) return;

        if(shootingCoroutine != null)
            StopCoroutine(shootingCoroutine);
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
    /// Mirrors the scale awesome style
    /// </summary>
    /// <returns></returns>
    private IEnumerator CorrectGunPosition()
    {   
        while(this != null)
        {
            if(inputDirection.x > 0)
                MirrorPivot.localScale = new Vector3(-1, 1, 1);
            
            if (inputDirection.x < 0)
                MirrorPivot.localScale = new Vector3(1, 1, 1);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Kind of unnessecary rn, but im cooking, ok
    /// </summary>
    public void UpdateShootingDirection()
    {
        //TODO
        //shootingDirection = inputDirection;

        if(PlayerControllerType.Equals(ControllerType.Keyboard))
        {
            UpdateShootingDirectionByKeyboard();
        }

        if (PlayerControllerType.Equals(ControllerType.Controller))
        {
            UpdateShootingDirectionByController();
        }
    }

    private void UpdateShootingDirectionByKeyboard()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        MousePosition -= (Vector2)transform.position;
        MousePosition = MousePosition.normalized;

        shootingDirection = MousePosition;
    }

    private void UpdateShootingDirectionByController()
    {
        shootingDirection = inputDirection;
    }

    private IEnumerator RepeatedFire()
    {
        while(primaryShooting)
        {
            ShootBullet();
            yield return new WaitForSeconds(SecondsBetweenShots);
        }
    }

    /// <summary>
    /// instantiates a bullet!
    /// this is gonna be bonkers once upgrades are added, i worry
    /// </summary>
    private void ShootBullet()
    {
        UpdateShootingDirection();

        //spawn the thing
        GameObject bullet = Instantiate(BulletPrefab, Gun.transform.position, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        //fix the thing
        bulletRigidbody.velocity = shootingDirection * BulletVelocity;
        float Angle = Vector2.SignedAngle(Vector2.right, shootingDirection);

        Vector3 TargetRotation = new Vector3(0, 0, Angle);
        bullet.transform.eulerAngles = TargetRotation;
    }
}
