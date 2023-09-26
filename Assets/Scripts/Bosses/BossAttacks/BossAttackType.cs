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
    protected PlayerBehaviour playerBehaviour;
    protected GameObject Player;

    [Tooltip("If the boss attack happens on start")]
    public bool StartAttackCycleOnAwake; //actually on start tho

    protected bool CurrentlyAttacking;
    protected Coroutine AttackCoroutine;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    public virtual void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        Player = playerBehaviour.gameObject;

        if(StartAttackCycleOnAwake)
        {
            StartAttack();
        }
    }
    
    /// <summary>
    /// Activates the attack!
    /// </summary>
    public virtual void StartAttack()
    {
        CurrentlyAttacking = true;
    }

    /// <summary>
    /// Deactivates the attack!
    /// </summary>
    public virtual void StopAttack()
    {
        CurrentlyAttacking = false;

        if (AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);
    }
    
}
