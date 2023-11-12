/*******************************************************************************
* File Name :         SharpShooterUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/12/2023
*
* Brief Description : Makes the players bullets pierce enemies. Also draws a aim line.
* A seperate component makes the bullets do more damage after every piercing.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpShooterUpgrade : UpgradeType
{
    public LayerMask LM;

    private Transform gun;

    private LineRenderer lineRenderer;
    

    protected override void Start()
    {
        base.Start();

        //such risky code but i am too lazy to do better rn
        gun = GameObject.Find("Gun").transform;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        StartCoroutine(DrawAimLine());
    }
    public override void UpgradeEffect(AttackType bullet)
    {
        bullet.DestroyedAfterAttack = false;

        bullet.gameObject.AddComponent<ProjectilePiercing>();
    }

    private IEnumerator DrawAimLine()
    {
        while(true)
        {
            if(PlayerBehaviour.Instance.WeaponSelected == PlayerBehaviour.Weapon.Gun)
            {
                lineRenderer.enabled = true;

                Vector3 wallPoint = GetAimingPosition();

                lineRenderer.SetPosition(0, gun.position);
                lineRenderer.SetPosition(1, wallPoint);
            }

            if (PlayerBehaviour.Instance.WeaponSelected == PlayerBehaviour.Weapon.Sword)
            {
                lineRenderer.enabled = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// returns the posint on the wall the player is aiming at
    /// </summary>
    private Vector2 GetAimingPosition()
    {
        //Vector2 direction = (Vector2)Random.rotation.eulerAngles;
        Vector3 direction = PlayerController.Instance.AimingDirection;

        direction.Normalize();

        //Debug.Log("Direction_is_" + direction);


        RaycastHit2D hitInfo = Physics2D.Raycast(gun.position, direction, 50, LM);

        if (hitInfo.transform != null)
        {
            return hitInfo.point;
        }

        else //if it hit nothing:
        {
            return gun.transform.position + (direction * 50);
        }
    }
}
