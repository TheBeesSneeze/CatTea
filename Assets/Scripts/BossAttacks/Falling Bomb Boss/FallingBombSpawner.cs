/*******************************************************************************
* File Name :         FallingBombSpawner.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/30/2023
*
* Brief Description : RandomAttackSpawner, updates bosses animation
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBombSpawner : RandomAttackSpawner
{

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override void PerformAttack()
    {
        base.PerformAttack();

        if (animator != null)
        {
            animator.SetTrigger("FireMortar");
        }
    }
}
