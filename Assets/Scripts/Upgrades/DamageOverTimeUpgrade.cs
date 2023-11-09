/*******************************************************************************
* File Name :         DamageOverTimeUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/14/2023
*
* Brief Description : Makes enemy do what the name of the script says.
* DamageOverTime.cs has functionality that makes half this code redudnant.
* I am too lazy to change it however
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageOverTimeUpgrade : UpgradeType
{
    [Header("DOT Settings")]

    [Tooltip("between 0 and 1. Chance enemy will take DOT")]
    public float DOTChance;
    private float _DOTChance;

    [Tooltip("How much damage will be taken after effect ends")]
    public float TotalDamage;

    [Tooltip("How long effect lasts")]
    public float DOTDuration;

    public Color EnemyColor = new Color(0.25f, 0.7f, 0.9f);

    public override void UpgradeEffect(CharacterBehaviour eventCharacter)
    {
        float r = Random.value;

        if(r <= DOTChance)
        {
            Debug.Log("dot!");
            StartCoroutine(DamageOverTime(eventCharacter));
        }
    }

    public override void DuplicateUpgradeEffect()
    {
        DamageOverTimeUpgrade originalDOT = originalUpgrade.GetComponent<DamageOverTimeUpgrade>();

        originalDOT.DOTChance 
    }

    /// <summary>
    /// Adds and removes DamageOverTime component
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageOverTime(CharacterBehaviour eventCharacter)
    {
        DamageOverTime DOT = eventCharacter.AddComponent<DamageOverTime>();
        DOT.Initialize(TotalDamage / DOTDuration, EnemyColor);

        yield return new WaitForSeconds(DOTDuration);

        DOT.Stop();
    }
}
