/*******************************************************************************
* File Name :         MeleePlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* "Brief" Description : Code mostly stolen from Gorp Game. Shoutouts to which
* ever TA wrote the sword swinging code.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleePlayerController : DefaultPlayerController
{
    [Header("Settings")]
    public float AttackLength;
    public float StrikeFrames = 30;

    [Header("Unity Stuff")]
    public Collider2D Sword;
    public Transform RotatePoint;

    [Header("Controls")]
    private Quaternion swordRotation;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(RotateSword());
    }

    protected override void Primary_performed(InputAction.CallbackContext obj)
    {
        //if not already attacking
        if (!Sword.enabled)
        {
            Sword.enabled = true;

            StartCoroutine(SwingSword());

            if (GameManager.Instance.Rumble && myGamepad != null)
            {
                //InputDevice a = MyPlayerInput.devices[0];
                myGamepad.SetMotorSpeeds(0.15f, 0.25f);
            }
        }
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
        //TODO
    }

    /// <summary>
    /// swings sword all sword like
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwingSword()
    {
        Vector3 originalPoint = RotatePoint.rotation.eulerAngles;
        Vector3 startAngle = RotatePoint.rotation.eulerAngles;
        startAngle.z += 45;
        Vector3 endAngle = RotatePoint.rotation.eulerAngles;
        endAngle.z += -45;

        for (int i = 0; i < StrikeFrames; i++)
        {
            Vector3 target = Vector3.Lerp(startAngle, endAngle, i / StrikeFrames);
            RotatePoint.transform.eulerAngles = target;

            yield return new WaitForSeconds(AttackLength / StrikeFrames);
        }

        //RotatePoint.transform.eulerAngles = originalPoint;
        RotatePoint.rotation = swordRotation;

        StopAttack();
    }

    /// <summary>
    /// stops attack - turns sword inactive
    /// </summary>
    private void StopAttack()
    {
        if (GameManager.Instance.Rumble && myGamepad != null)
            myGamepad.SetMotorSpeeds(0, 0);

        Sword.enabled = false;
    }

    /// <summary>
    /// rotates sword towards the players movement direction using a lot of cool math.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateSword()
    {
        //this could be a function, it should be called whenever the player moves. but its also gotta be constant
        while(this != null)
        {
            //float angle = Mathf.Atan2(move.ReadValue<Vector2>().y, move.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            Debug.Log(angle);

            swordRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            RotatePoint.rotation = swordRotation;

            yield return new WaitForSeconds(0.02f);
        }

        //i KNOW this function will cause issues in the future
        //leaving these comments for whoever gets the pleasure of fixing it :)
    }
}
