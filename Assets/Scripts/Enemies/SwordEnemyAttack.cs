/*******************************************************************************
* File Name :         SwordEnemyAttack.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/29/2023
*
* Brief Description : Functionally very similar to SwordBossAttack. I am too tired
* and crunched and lazy to do this properly
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordEnemyAttack : MonoBehaviour
{
    public SwordEnemy swordEnemy;
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

    //magic numbers
    private float shakeSwordAmount = 0.05f;

    //private bool playerInsideRange;
    private bool swordAttackInProgress;
    private Quaternion swordRotation;

    private MeleePlayerController meleePlayer;

    private NavMeshAgent navMeshAgent;
    private SpriteRenderer swordSpriteRenderer;
    private Collider2D swordActivationTrigger;

    private Vector2 defaultSwordPosition;
    private Vector3 defaultSwordScale;


    // Start is called before the first frame update
    private void Start()
    {
        navMeshAgent = swordEnemy.GetComponent<NavMeshAgent>();
        swordSpriteRenderer = SwordCollider.GetComponent<SpriteRenderer>();
        swordActivationTrigger = GetComponent<Collider2D>();

        meleePlayer = GameObject.FindObjectOfType<MeleePlayerController>();
        swordAngle = meleePlayer.PrimaryStrikeAngle;

        defaultSwordPosition = SwordCollider.transform.localPosition;
        defaultSwordScale = SwordCollider.transform.localScale;

        swordSpriteRenderer.enabled = false;
    }

    /// <summary>
    /// Phase A: Gets ready to strike. Stops movement and ranged attacks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadySwordAttack()
    {
        swordAttackInProgress = true;
        EnableSword();
        DisableMovement();

        float t = 0; // 0 <= t <= ReadySwordSeconds
        while (t < ReadySwordSeconds)
        {
            t += Time.deltaTime;

            AngleSwordAtPlayer(t / ReadySwordSeconds);
            ExpandSword(t / ReadySwordSeconds);
            MoveTowardsPlayer();
            ShakeSword();

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
        SwordCollider.transform.localPosition = defaultSwordPosition;

        Vector3 startAngle = transform.rotation.eulerAngles;
        Vector3 endAngle = transform.rotation.eulerAngles;

        endAngle.z += swordAngle * 2;

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
        swordActivationTrigger.enabled = false;

        yield return new WaitForSeconds(IdleAfterAttackSeconds);

        EnableMovement();

        yield return new WaitForSeconds(SwordAttackCooldown);

        swordActivationTrigger.enabled = true;

        swordAttackInProgress = false;
    }

    /// <summary>
    /// angles the sword towards the player, -90 degress
    /// </summary>
    private void AngleSwordAtPlayer(float percent)
    {
        Vector2 aimingDirection = GetPlayerDirection();

        //float angle = Mathf.Atan2(move.ReadValue<Vector2>().y, move.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg;
        angle -= (swordAngle / 2) + ((swordAngle / 2) * percent);

        swordRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = swordRotation;

        UpdateSwordMirrorDirection();
    }

    private Vector2 GetPlayerDirection()
    {
        Vector2 difference = PlayerBehaviour.Instance.transform.position - swordEnemy.transform.position;
        return difference.normalized;
    }

    private void UpdateSwordMirrorDirection()
    {
        //idk figure it out
    }

    /// <summary>
    /// scales the sword from 0 to whatever it is at start
    /// </summary>
    /// <param name="percent"></param>
    private void ExpandSword(float percent)
    {
        float percentScaled = Mathf.Pow(percent, 1f / 4f);
        SwordCollider.transform.localScale = defaultSwordScale * percentScaled;
    }

    /// <summary>
    /// randomly shakes the sword 
    /// </summary>
    private void ShakeSword()
    {
        float x = Random.Range(-shakeSwordAmount, shakeSwordAmount);
        float y = Random.Range(-shakeSwordAmount, shakeSwordAmount);

        SwordCollider.transform.localPosition = defaultSwordPosition + new Vector2(x, y);
    }

    /// <summary>
    /// inches towards player, tries to keep a distance of TargetPlayerDistance
    /// </summary>
    private void MoveTowardsPlayer()
    {
        Vector2 bossPosition = swordEnemy.transform.position;
        Vector2 playerPositon = meleePlayer.transform.position;

        Vector2 moveTowardPlayerPosition = Vector2.MoveTowards(bossPosition, playerPositon, ApproachPlayerMovementSpeed * Time.deltaTime);

        float distance = Vector2.Distance(moveTowardPlayerPosition, playerPositon);

        if (distance < TargetPlayerDistance)
            return;

        swordEnemy.transform.position = moveTowardPlayerPosition;
    }

    /// <summary>
    /// stop boss movement
    /// </summary>
    private void EnableSword()
    {
        SwordCollider.transform.localPosition = defaultSwordPosition;
        swordSpriteRenderer.enabled = true;
    }
    private void DisableMovement()
    {
        navMeshAgent.enabled = false;   
    }

    /// <summary>
    /// called after sword swings
    /// </summary>
    private void DisableSword()
    {
        swordSpriteRenderer.enabled = false;
        SwordCollider.enabled = false;
    }

    private void EnableMovement()
    {
        navMeshAgent.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if (tag.Equals("Player"))
        {
            //playerInsideRange = true;

            if (!swordAttackInProgress)
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
            //playerInsideRange = false;
        }
    }
}
