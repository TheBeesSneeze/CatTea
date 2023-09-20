/*******************************************************************************
* File Name :         BossAttackType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/18/2023
*
* Brief Description : very different from AttackType. Base class for autonomous
* boss attacks
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackType : MonoBehaviour
{
    protected PlayerBehaviour playerBehaviour;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
    }
    

    
}
