/*******************************************************************************
* File Name :         DamageOverTime.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/27/23
*
* Brief Description : Damages the character attached to this gameobject for as
* long as this object exists.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CharacterBehaviour characterBehaviour;

    private Color originalColor;
    private Color overrideColor;
    private void Start()
    {
        Debug.Log("Damaging over second");

        characterBehaviour = GetComponent<CharacterBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        overrideColor = originalColor;
    }

    /// <summary>
    /// Stops dealing damage. Destroys the DOT component. Resets the color
    /// </summary>
    public void Stop()
    {
        characterBehaviour.colorOverride = characterBehaviour.originalColor;
        spriteRenderer.color = characterBehaviour.originalColor;

        StopAllCoroutines();

        Destroy(this);
    }

    public void Initialize(float damagePerSecond)
    {
        Debug.Log("DOT initialized! dealing " + damagePerSecond + " every second.");

        //have you ever CALLED the start function before??? I HOPE NOT. you have to be at a WHOLE NEW KIND OF LOW to be where i am at right now.
        Start();

        StartCoroutine(DealDamageOverTime(damagePerSecond));
    }

    public void Initialize(float damagePerSecond, Color NewColor)
    {
        Debug.Log("DOT initialized! dealing " + damagePerSecond + " every second.");

        //TWICE???? *turns to alcohol as a coping mechanism*
        Start();

        overrideColor = NewColor;
        characterBehaviour.colorOverride = NewColor;

        StartCoroutine(DealDamageOverTime(damagePerSecond));
    }

    /// <summary>
    /// deals damage ever 1/2 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator DealDamageOverTime(float damagePerSecond)
    {
        while(this != null)
        {
            Debug.Log("Damaging!");
            spriteRenderer.color = overrideColor;

            characterBehaviour.TakeDamage(damagePerSecond / 2, false);

            yield return new WaitForSeconds(0.5f);
        }

    }
}
