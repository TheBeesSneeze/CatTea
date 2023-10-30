/*******************************************************************************
* File Name :         BossAttackType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/18/2023
*
* Brief Description : very different from AttackType!! Base class for autonomous
* boss attacks
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackType : MonoBehaviour
{
    [Header("Settings")]

    [Tooltip("If the boss attack happens on start")]
    public bool StartAttackCycleOnAwake; //actually on start tho

    [Tooltip("If the attack will be repeated. If false, ignore all attacks below")]
    public bool LoopAttack;

    [Tooltip("Seconds between waves of attacks")]
    public float AttackCycleCooldown;
    [Tooltip("Seconds between each call of PerformAtttack in a cycle")]
    public float AttackInterval;
    [Tooltip("# of attack objects spawned per cycle")]
    public float AttacksPerCycle;

    protected BossBehaviour bossBehaviour;
    protected Animator ratbossAnimator;

    protected PlayerBehaviour playerBehaviour;
    protected GameObject Player;

    protected bool CurrentlyAttacking;
    protected Coroutine AttackCoroutine;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        Player = playerBehaviour.gameObject;

        bossBehaviour = GetComponent<BossBehaviour>();
        ratbossAnimator = GetComponent<Animator>();

        if (StartAttackCycleOnAwake)
        {
            StartAttack();
        }
    }

    /// <summary>
    /// What happens when the attack
    /// </summary>
    public virtual void PerformAttack()
    {
        Debug.Log("Override me");
    }

    /// <summary>
    /// Activates the attack!
    /// </summary>
    public virtual void StartAttack()
    {
        CurrentlyAttacking = true;

        if(LoopAttack)
            AttackCoroutine = StartCoroutine(AttackLoop());

        else
            PerformAttack();
    }

    /// <summary>
    /// Deactivates the attack!
    /// </summary>
    public virtual void StopAttack()
    {
        CurrentlyAttacking = false;

        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            AttackCoroutine = null;
        }
    }
    
    private IEnumerator AttackLoop()
    {
        
        while (CurrentlyAttacking)
        {
            yield return new WaitForSeconds(AttackCycleCooldown/2);

            int attacks = 0;
            while (attacks < AttacksPerCycle)
            {
                attacks++;

                PerformAttack();

                yield return new WaitForSeconds(AttackInterval);
            }

            yield return new WaitForSeconds(AttackCycleCooldown/2);
        }

        AttackCoroutine = null;
    }


}
