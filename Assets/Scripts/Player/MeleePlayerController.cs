/*******************************************************************************
* File Name :         MeleePlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* "Brief" Description : Code mostly stolen from Gorp Game. Shoutouts to which
* ever TA wrote the sword swinging code.
* 
* TODO: rumble
* Make sword stop more smooth
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleePlayerController : DefaultPlayerController
{
    [Header("Settings")]
    public float PrimaryStrikeAngle = 90;
    public float StrikeFrames = 10;

    [Header("Unity Stuff")]
    public Collider2D SwordCollider;
    public Transform RotatePoint;

    [Header("Controls")]
    private Quaternion swordRotation;
    private bool Attacking;
    private Coroutine RotatingSwordCoroutine;

    protected override void Start()
    {
        base.Start();
        RotatingSwordCoroutine = StartCoroutine(RotateSword());
    }

    protected override void Primary_performed(InputAction.CallbackContext obj)
    {
        if(IgnoreAllInputs) return;

        //if not already attacking
        if (Attacking || !canAttack)
        {
            return;
        }

        SwordCollider.enabled = true;
        Attacking = true;
        canAttack = false;

        StartCoroutine(HoldMeleeAttack());

        /*
        if (GameManager.Instance.Rumble && MyGamepad != null)
        {
            //InputDevice a = MyPlayerInput.devices[0];
            MyGamepad.SetMotorSpeeds(0.15f, 0.25f);
        }
        */
        
    }
    protected override void Primary_canceled(InputAction.CallbackContext obj)
    {
<<<<<<< Updated upstream
        if (IgnoreAllInputs) return;
        //TODO
=======
        StartCoroutine(StopAttack());
>>>>>>> Stashed changes
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

    private IEnumerator HoldMeleeAttack()
    {
        Vector3 startAngle = RotatePoint.rotation.eulerAngles;
        Vector3 endAngle = RotatePoint.rotation.eulerAngles;

        startAngle.z += PrimaryStrikeAngle / 2;
        endAngle.z += -PrimaryStrikeAngle / 2;

        while(Attacking)
        {
            for (int i = 0; i < StrikeFrames && Attacking; i++)
            {
                Vector3 target = Vector3.Lerp(startAngle, endAngle, i / StrikeFrames);
                RotatePoint.transform.eulerAngles = target;

                yield return new WaitForSeconds(playerBehaviour.PrimaryAttackSpeed / StrikeFrames);
            }

            for (int i = 0; i < StrikeFrames && Attacking; i++)
            {
                Vector3 target = Vector3.Lerp(endAngle, startAngle, i / StrikeFrames);
                RotatePoint.transform.eulerAngles = target;

                yield return new WaitForSeconds(playerBehaviour.PrimaryAttackSpeed / StrikeFrames);
            }
        }

        
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
        
        SwordCollider.enabled = false;
        Attacking = false;

        /*

        float moveSwordBackSeconds = playerBehaviour.PrimaryAttackCoolDown / 2;

        float startAngle = RotatePoint.transform.eulerAngles.z;
        float targetAngle = (Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg);

        startAngle = (startAngle+360) % 360;
        targetAngle = (targetAngle+360) % 360;

        Debug.Log(startAngle);
        Debug.Log(targetAngle);

        //return sword to where it started
        for (int i = 0; i < 20; i++) 
        {
            float target = Mathf.Lerp(startAngle, targetAngle, i / StrikeFrames);
            RotatePoint.transform.eulerAngles = new Vector3(0,0,target);

            yield return new WaitForSeconds(moveSwordBackSeconds / 20);
        }
        */

        RotatingSwordCoroutine = StartCoroutine(RotateSword());

        yield return new WaitForSeconds(playerBehaviour.PrimaryAttackCoolDown/2);
        canAttack = true;
    }

    /// <summary>
    /// rotates sword towards the players movement direction using a lot of cool math.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateSword()
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        float lastAngle;

        //this could be a function, it should be called whenever the player moves. but its also gotta be constant
        while(!Attacking)
        {
            lastAngle = angle;
            //float angle = Mathf.Atan2(move.ReadValue<Vector2>().y, move.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
            angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            angle += 360;

            if (angle != lastAngle)
            {
                //kinda smoothen the angle
                float difference = Mathf.Abs(angle - lastAngle);
                if (difference <= 95 )
                    angle = (angle + lastAngle) / 2;

                swordRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                RotatePoint.rotation = swordRotation;
            }

            yield return new WaitForSeconds(0.02f);
        }

        RotatingSwordCoroutine = null;

        //i KNOW this function will cause issues in the future
        //leaving these comments for whoever gets the pleasure of fixing it :)
    }
}
