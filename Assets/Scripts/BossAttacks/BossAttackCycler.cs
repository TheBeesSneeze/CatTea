/*******************************************************************************
* File Name :         BossAttackCycler.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/30/2023
*
* Brief Description : Runs each boss attack with delays. Uses AttackCycleCooldown from
* attacks to determine cooldown between each cycle
*******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCycler : MonoBehaviour
{
    [Tooltip("Boss attacks cycle in order")]
    public List<BossAttackType> BossAttacks;

    //[Tooltip("Seconds between each attack, synced with BossAttacks.")]
    //public List<float> SecondsBetweenAttacks;
    public bool CycleOnStart = true;

    private Coroutine loopCoroutine;
    private bool Cycling;

    void Start()
    {
        if(CycleOnStart)
            StartCycle();
    }
    public void StartCycle()
    {
        Cycling = true;
        if(loopCoroutine != null)
        {
            Debug.Log("Restarting attack cycle");
            StopCoroutine(loopCoroutine);
        }

        StartCoroutine(LoopThroughAttacks());
    }

    public void EndCycle()
    {
        Cycling = false;
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
        }
        loopCoroutine = null;
    }
    private IEnumerator LoopThroughAttacks()
    {
        //so the player doesnt die immediately
        float cycleSeconds = 1;

        while (Cycling)
        {
            foreach (BossAttackType bossAttack in BossAttacks) 
            {
                yield return new WaitForSeconds(cycleSeconds);

                cycleSeconds = bossAttack.StartAttackCycle();

                yield return new WaitForSeconds(bossAttack.AttackCycleCooldown);
            }
        }

        EndCycle();
    }
}
