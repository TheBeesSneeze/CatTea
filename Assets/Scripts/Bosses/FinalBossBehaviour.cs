/*******************************************************************************
* File Name :         FinalBossBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/29/23
*
* Brief Description : rotates gun to face player. spawns in sawblade.
* 
* spawns in another sawblade at 1/2 health
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class FinalBossBehaviour : BossBehaviour
{
    [Header("Final Boss")]
    public GameObject SawbladePrefab;

    protected Vector2 enemyDirection;

    public Transform GunPivot;
    public Transform GunSprite;

    [HideInInspector]public Vector2 AimingDirection; //normalized vector2 that points towards player
    private int sawsSpawned = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(RotateGunTowardsPlayer());


        StartCoroutine(UpdateAnimation());
    }

    public override void Initialize()
    {
        base.Initialize();

        SpawnSawblade();
    }

    /// <summary>
    /// spawns saw blade if at 2/3 health and 1/3 health
    /// </summary>
    /// <param name="value"></param>
    public override void SetHealth(float value)
    {
        base.SetHealth(value);

        if (sawsSpawned >= 3)
            return;

        if (HealthPoints <= StartHealth * (2f / 3f) && sawsSpawned == 1)
        {
            SpawnSawblade();
            sawsSpawned++;
            return;
        }

        if (HealthPoints <= StartHealth * (1f / 3f) && sawsSpawned == 2)
        {
            SpawnSawblade();
            sawsSpawned++;
            return;
        }
    }

    public void SpawnSawblade()
    {
        GameObject sawblade = Instantiate(SawbladePrefab, transform.position, SawbladePrefab.transform.rotation);
        AttacksSpawned.Add(sawblade);
        sawsSpawned++;
    }

    private IEnumerator RotateGunTowardsPlayer()
    {
        while(this!= null)
        {
            Vector2 playerPosition = PlayerBehaviour.Instance.transform.position;

            playerPosition -= (Vector2)transform.position;
            playerPosition = playerPosition.normalized;

            AimingDirection = playerPosition;

            //rotate awesome style
            playerPosition = new Vector2(playerPosition.x, playerPosition.y);
            float angle = Mathf.Atan2(playerPosition.y, playerPosition.x) * Mathf.Rad2Deg;

            GunPivot.localEulerAngles = new Vector3(0, 0, angle);

            RotateGunSprite(playerPosition);

            yield return null;
        }
    }

    private void RotateGunSprite(Vector2 playerPosition)
    {
        //mirror gun
        Vector3 scale = GunSprite.localScale;
        if (playerPosition.x > 0)
        {
            scale.y = Mathf.Abs(scale.x);
        }
        if (playerPosition.x < 0)
        {
            scale.y = -Mathf.Abs(scale.x);
        }
        GunSprite.localScale = scale;
    }

    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            Vector2 playerPosition = PlayerBehaviour.Instance.transform.position;

            playerPosition -= (Vector2)transform.position;
            playerPosition = playerPosition.normalized;

            Vector2 aim = playerPosition;

            MyAnimator.SetFloat("XMovement", aim.x);
            MyAnimator.SetFloat("YMovement", aim.y);
            yield return null;
        }
    }
}
