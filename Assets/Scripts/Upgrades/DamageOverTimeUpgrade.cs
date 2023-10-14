/*******************************************************************************
* File Name :         DamageOverTimeUpgrade.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/14/2023
*
* Brief Description : Makes enemy do what the name of the script says.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeUpgrade : UpgradeType
{
    [Header("DOT Settings")]

    [Tooltip("between 0 and 1. Chance enemy will take DOT")]
    public float DOTChance;

    [Tooltip("How much damage will be taken after effect ends")]
    public float TotalDamage;

    [Tooltip("How long effect lasts")]
    public float DOTDuration;

    //time between each damage
    private float damageInterval=0.5f;

    public override void UpgradeEffect(CharacterBehaviour eventCharacter)
    {
        float r = Random.value;

        Debug.Log(r);

        if(r <= DOTChance)
        {
            Debug.Log("dot!");
            StartCoroutine(DamageOverTime(eventCharacter));
        }
    }

    private IEnumerator DamageOverTime(CharacterBehaviour eventCharacter)
    {
        SpriteRenderer enemySprite = eventCharacter.GetComponent<SpriteRenderer>();
        Color oldColor = enemySprite.color;

        enemySprite.color = new Color(0.25f, 0.7f, 0.9f);

        int iterations = (int)(DOTDuration / damageInterval);
        int i = 0;

        while (i < iterations && eventCharacter != null)
        {
            if(eventCharacter.HealthPoints > 0)
            {
                Debug.Log("Damage on time woo");
                eventCharacter.TakeDamage(TotalDamage / ((float)iterations), false);
            }
            

            yield return new WaitForSeconds(damageInterval);
        }

        if(enemySprite != null)
            enemySprite.color = oldColor;
    }
}
