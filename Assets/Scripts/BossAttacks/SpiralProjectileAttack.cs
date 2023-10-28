/*******************************************************************************
* File Name :         SpiralProjectileAttack.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/18/2023
*
* Brief Description : spawns projectiles from an invisible gameobject which is always spinning
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralProjectileAttack : BossAttackType
{
    public GameObject SpiralBulletPrefab;
    public float ProjectileSpeed;

    public Transform RotationPivot;
    public Transform BulletSpawnPoint;

    public override void PerformAttack()
    {
        Vector2 bulletVelocity = (BulletSpawnPoint.position - RotationPivot.position).normalized * ProjectileSpeed;

        GameObject newBullet = Instantiate(SpiralBulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D newBulletRB = newBullet.GetComponent<Rigidbody2D>();

        newBulletRB.velocity = bulletVelocity;

        newBullet.transform.eulerAngles = RotationPivot.eulerAngles;
    }
}
