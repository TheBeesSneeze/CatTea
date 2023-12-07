/*******************************************************************************
* File Name :         GunBossAttack.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/18/2023
*
* Brief Description : Replicates the players gun attack. 
* Streals a lot of data from the player
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBossAttack : BossAttackType
{
    [Header("Gun settings")]
    public GameObject BulletPrefab;

    private float projectileSpeed;
    private FinalBossBehaviour finalBossBehaviour;

    // Start is called before the first frame update
    protected override void Start()
    {
        finalBossBehaviour = GetComponent<FinalBossBehaviour>();

        StealPlayerStats();

        base.Start();
    }

    /// <summary>
    /// translates player stats to bossattack stats.
    /// </summary>
    public void StealPlayerStats()
    {
        Debug.Log("Stealing stats");
        projectileSpeed = PlayerBehaviour.Instance.ProjectileSpeed;
        AttacksPerCycle = PlayerBehaviour.Instance.ShotsPerBurst;
        AttackInterval = PlayerBehaviour.Instance.TimeBetweenShots;
        AttackCycleCooldown = PlayerBehaviour.Instance.AmmoRechargeTime * AttacksPerCycle;
    }

    public override void PerformAttack()
    {
        //spawn the thing
        GameObject bullet = Instantiate(BulletPrefab, finalBossBehaviour.GunSprite.position, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        bulletRigidbody.velocity = finalBossBehaviour.AimingDirection * projectileSpeed;

        //rotate
        float Angle = Vector2.SignedAngle(Vector2.right, finalBossBehaviour.AimingDirection);
        Vector3 TargetRotation = new Vector3(0, 0, Angle);
        bullet.transform.eulerAngles = TargetRotation;

        //ShootSound.Play();

        //UpdateGunSprite();
    }
}
