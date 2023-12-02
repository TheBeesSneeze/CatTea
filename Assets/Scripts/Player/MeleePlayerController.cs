/*******************************************************************************
* File Name :         MeleePlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Singleton. Code mostly stolen from Gorp Game. Shoutouts to which
* ever TA wrote the sword swinging code.
* 
* TODO: rumble
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleePlayerController : MonoBehaviour
{
    public static MeleePlayerController Instance;

    [Header("Settings")]
    public float PrimaryStrikeAngle = 120;

    [Header("Unity Stuff")]
    public Collider2D SwordCollider;
    public Transform RotatePoint;
    public Animator SwordAnimator;

    [Header("Controls")]
    private Quaternion swordRotation;
    private bool Attacking;

    // le sound
    public AudioSource SwordSlash;

    protected void Start()
    {
         StartCoroutine(RotateSword());
    }

    public void Sword_started(InputAction.CallbackContext obj)
    {
        if(PlayerController.Instance.IgnoreAllInputs) 
            return;

        //if not already attacking
        if (Attacking && !RangedPlayerController.Instance.PrimaryShooting)
            return;

        if (!PlayerController.Instance.CanAttack)
            return;

        PlayerBehaviour.Instance.StartSwordMode();

        GameEvents.Instance.OnPlayerSword();

        SwordAnimator.SetBool("Attacking", true);

        SwordCollider.enabled = true;
        Attacking = true;
        PlayerController.Instance.CanAttack = false;

        PlaySwordSound();

        StartCoroutine(MeleeAttack());

        /*
        if (GameManager.Instance.Rumble && MyGamepad != null)
        {
            //InputDevice a = MyPlayerInput.devices[0];
            MyGamepad.SetMotorSpeeds(0.15f, 0.25f);
        }
        */
        
    }
    public void Sword_canceled(InputAction.CallbackContext obj)
    {
        if (PlayerController.Instance.IgnoreAllInputs) return;
        //TODO
        //StartCoroutine(StopAttack());

        //playerController.StartGunMode();
    }
    

    private IEnumerator MeleeAttack()
    {
        PlayerBehaviour.Instance.Speed = PlayerBehaviour.Instance.Speed/2;

        Vector3 startAngle = RotatePoint.rotation.eulerAngles;
        Vector3 endAngle = RotatePoint.rotation.eulerAngles;

        startAngle.z += PrimaryStrikeAngle / 1.5f;
        endAngle.z += -PrimaryStrikeAngle / 1.5f;

        float t = 0;
        while(t < PlayerBehaviour.Instance.TimeBetweenShots)
        {
            t += Time.deltaTime;

            Vector3 target = Vector3.Lerp(startAngle, endAngle, t / PlayerBehaviour.Instance.TimeBetweenShots);
            RotatePoint.transform.eulerAngles = target;

            yield return null;
        }

        PlayerBehaviour.Instance.Speed = PlayerBehaviour.Instance.CurrentPlayerStats.Speed;
        StartCoroutine(StopAttack());
    }

    /// <summary>
    /// TODO: THIS CODE DOESNT REALLY WORK THAT WELL YET
    /// 
    /// stops attack - turns sword inactive
    /// rotates sword back to where it goes oh yeah
    /// </summary>
    private IEnumerator StopAttack()
    {
        //if (GameManager.Instance.Rumble && MyGamepad != null)
        //    MyGamepad.SetMotorSpeeds(0, 0);

        SwordAnimator.SetBool("Attacking", false);

        SwordCollider.enabled = false;
        Attacking = false;

        StartCoroutine(RotateSword());

        yield return new WaitForSeconds(PlayerBehaviour.Instance.SwordAttackCoolDown);
        PlayerController.Instance.CanAttack = true;
    }

    /// <summary>
    /// rotates sword towards the players movement direction using a lot of cool math.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateSword()
    {
        //float angle = Mathf.Atan2(playerController.MoveDirection.y, playerController.MoveDirection.x) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(PlayerController.Instance.AimingDirection.y, PlayerController.Instance.AimingDirection.x) * Mathf.Rad2Deg;
        float lastAngle;

        //this could be a function, it should be called whenever the player moves. but its also gotta be constant
        while(!Attacking)
        {
            lastAngle = angle;
            //float angle = Mathf.Atan2(move.ReadValue<Vector2>().y, move.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
            angle = Mathf.Atan2(PlayerController.Instance.AimingDirection.y, PlayerController.Instance.AimingDirection.x) * Mathf.Rad2Deg;
            angle += 360;

            if (angle != lastAngle && !PlayerController.Instance.IgnoreAllInputs)
            {
                //kinda smoothen the angle
                float difference = Mathf.Abs(angle - lastAngle);
                if (difference <= 95 )
                    angle = (angle + lastAngle) / 2;

                swordRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                RotatePoint.rotation = swordRotation;
            }

            UpdateSwordMirrorDirection();

            yield return new WaitForEndOfFrame();
        }

        //i KNOW this function will cause issues in the future
        //leaving these comments for whoever gets the pleasure of fixing it :)
    }

    public void UpdateSwordMirrorDirection()
    {
        float xDifference = SwordCollider.transform.position.x - transform.position.x;

        if (xDifference >= 0)
        {
            SwordCollider.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            SwordCollider.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    /// <summary>
    /// randomizes pitch
    /// </summary>
    private void PlaySwordSound()
    {
        SwordSlash.pitch = Random.Range(0.75f, 1.25f);
        SwordSlash.Play();
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
