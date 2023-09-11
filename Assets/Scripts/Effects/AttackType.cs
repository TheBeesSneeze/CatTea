/*******************************************************************************
* File Name :         AttackType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/7/2023
*
* Brief Description :
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackType : MonoBehaviour
{
    [Tooltip("true if this bullet came from an enemy")]
    public bool EnemyAttack;

    public int Damage;
    
}
