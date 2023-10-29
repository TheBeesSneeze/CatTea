/*******************************************************************************
* File Name :         SwordBossAttack.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/29/2023
*
* Brief Description : Does not extend boss attack type. Enables a sword when the
* player gets close.
* Gives the player a second to anticipate the sword. slows down while aiming.
* 
* might disable the gun when it goes sword mode
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBossAttack : MonoBehaviour
{
    public Collider2D SwordCollider;
    [Tooltip("Speed of the boss when it is walking towards player")]
    public float ApproachPlayerMovementSpeed;
    [Tooltip("How far away to be when moving towards player")]
    public float TargetPlayerDistance;
    [Tooltip("Time until boss attacks when player gets close")]
    public float ReadySwordSeconds;
    [Tooltip("Time it takes for boss to swing sword")]
    public float SwingSwordSeconds;
    [Tooltip("Seconds until everything else resumes")]
    public float IdleAfterAttackSeconds;
    [Tooltip("Seconds until another sword attack can happen")]
    public float SwordAttackCooldown;
    

    private float swordAngle; //steal from player

    private bool playerInsideRange;
    private bool swordAttackInProgress;
    private Quaternion swordRotation;

    private MeleePlayerController meleePlayer;

    private FinalBossBehaviour finalBossBehaviour;
    private GunBossAttack gunBossAttack;
    private MovementCycle movementCycle;
    private SpriteRenderer gunSpriteRenderer;
    private SpriteRenderer swordSpriteRenderer;

    private float defaultMoveSpeed;
    private Vector2 defaultSwordPosition;


    // Start is called before the first frame update
    private void Start()
    {
        finalBossBehaviour = GameObject.FindObjectOfType<FinalBossBehaviour>();
        gunBossAttack = finalBossBehaviour.GetComponent<GunBossAttack>();
        movementCycle = finalBossBehaviour.GetComponent<MovementCycle>();
        gunSpriteRenderer = finalBossBehaviour.GunSprite.GetComponent<SpriteRenderer>();
        swordSpriteRenderer = SwordCollider.GetComponent<SpriteRenderer>();

        meleePlayer = GameObject.FindObjectOfType<MeleePlayerController>();
        swordAngle = meleePlayer.PrimaryStrikeAngle;

        defaultMoveSpeed = finalBossBehaviour.MoveUnitsPerSecond;
        defaultSwordPosition = SwordCollider.transform.localPosition;
    }

    /// <summary>
    /// stop boss movement
    /// </summary>
    private void EnableSword()
    {
        SwordCollider.transform.localPosition = Vector3.zero;
        swordSpriteRenderer.enabled = true;
    }
    private void DisableGun()
    {
        gunSpriteRenderer.enabled = false;
        gunBossAttack.StopAttack();
    }
    private void DisableMovement()
    {
        finalBossBehaviour.MoveUnitsPerSecond = 0;
        movementCycle.StopAttack();
    }

    /// <summary>
    /// called after sword swings
    /// </summary>
    private void DisableSword() 
    {
        swordSpriteRenderer.enabled = false;
        SwordCollider.enabled = false;
    }
    
    private void EnableGun()
    {
        gunSpriteRenderer.enabled = true;
        gunBossAttack.StartAttack();
    }
    private void EnableMovement()
    {
        finalBossBehaviour.MoveUnitsPerSecond = defaultMoveSpeed;
        movementCycle.StartAttack();
    }

    

    /// <summary>
    /// angles the sword towards the player, -90 degress
    /// </summary>
    private void AngleSwordAtPlayer(float percent)
    {
        //float angle = Mathf.Atan2(move.ReadValue<Vector2>().y, move.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(finalBossBehaviour.AimingDirection.y, finalBossBehaviour.AimingDirection.x) * Mathf.Rad2Deg;
        angle -= (swordAngle/2) + ((swordAngle/2) * percent);

        swordRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = swordRotation;

        UpdateSwordMirrorDirection();
    }

    private void UpdateSwordMirrorDirection()
    {
        //idk figure it out
    }

    /// <summary>
    /// randomly shakes the sword 
    /// </summary>
    private void ShakeSword()
    {


        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);

        SwordCollider.transform.localPosition = defaultSwordPosition + new Vector2(x, y);
    }

    /// <summary>
    /// inches towards player, tries to keep a distance of TargetPlayerDistance
    /// </summary>
    private void MoveTowardsPlayer()
    {
        Vector2 bossPosition = finalBossBehaviour.transform.position;
        Vector2 playerPositon = meleePlayer.transform.position;

        Vector2 moveTowardPlayerPosition = Vector2.MoveTowards(bossPosition, playerPositon, ApproachPlayerMovementSpeed * Time.deltaTime);

        float distance = Vector2.Distance(moveTowardPlayerPosition, playerPositon);

        if (distance < TargetPlayerDistance)
            return;

        finalBossBehaviour.transform.position = moveTowardPlayerPosition;
    }

    /// <summary>
    /// Phase A: Gets ready to strike. Stops movement and ranged attacks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadySwordAttack()
    {
        swordAttackInProgress = true;
        EnableSword();
        DisableGun();
        DisableMovement();

        float t = 0; // 0 <= t <= ReadySwordSeconds
        while (t < ReadySwordSeconds)
        {
            t+= Time.deltaTime;

            AngleSwordAtPlayer(t/ ReadySwordSeconds);
            MoveTowardsPlayer();

            yield return null;
        }

        StartCoroutine(SwingSword());
    }

    /// <summary>
    /// Phase B: attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwingSword()
    {
        SwordCollider.enabled = true;

        Vector3 startAngle = transform.rotation.eulerAngles;
        Vector3 endAngle = transform.rotation.eulerAngles;

        endAngle.z += swordAngle*2;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / SwingSwordSeconds;

            Vector3 target = Vector3.Lerp(startAngle, endAngle, t);
            transform.eulerAngles = target;

            yield return null;
        }

        StartCoroutine(StopAttack());
    }

    /// <summary>
    /// Phase C: Reenables other boss attacks. starts cooldown until next attack can happen
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopAttack()
    {
        EnableMovement();

        yield return new WaitForSeconds(IdleAfterAttackSeconds);

        DisableSword();
        EnableGun();
        
        yield return new WaitForSeconds(SwordAttackCooldown);

        swordAttackInProgress = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if(tag.Equals("Player"))
        {
            playerInsideRange = true;

            if(!swordAttackInProgress)
            {
                StartCoroutine(ReadySwordAttack());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.tag;

        if (tag.Equals("Player"))
        {
            playerInsideRange = false;
        }
    }
}
