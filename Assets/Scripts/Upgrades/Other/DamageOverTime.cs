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

    public float damageOverTimeInterval = 1.5f;

    private void Start()
    {
        //Debug.Log("Damaging over second on " + gameObject.name);

        characterBehaviour = GetComponent<CharacterBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = characterBehaviour.colorOverride;
        overrideColor = originalColor;
    }

    /// <summary>
    /// Stops dealing damage. Destroys the DOT component. Resets the color
    /// </summary>
    public void Stop()
    {
        //Debug.Log("Stop damaging over second on " + gameObject.name);

        StopAllCoroutines();

        ResetCharacterColor();

        Destroy(this);
    }

    public void Initialize(float damagePerSecond)
    {
        //Debug.Log("DOT initialized! dealing " + damagePerSecond + " every second.");

        //have you ever CALLED the start function before??? I HOPE NOT. you have to be at a WHOLE NEW KIND OF LOW to be where i am at right now.
        Start();

        StartCoroutine(DealDamageOverTimeForever(damagePerSecond));
    }

    public void Initialize(float damagePerSecond, float totalSeconds)
    {
        //Debug.Log("DOT initialized! dealing " + damagePerSecond + " every second over "+ totalSeconds +" seconds");

        //TWICE???? *turns to alcohol as a coping mechanism*
        Start();

        StartCoroutine(DealDamageOverTime(damagePerSecond, totalSeconds));
    }

    public void Initialize(float damagePerSecond, float totalSeconds, Color NewColor)
    {
        //Debug.Log("DOT initialized! dealing " + damagePerSecond + " every second over " + totalSeconds + " seconds");

        Start();

        overrideColor = NewColor;
        characterBehaviour.colorOverride = NewColor;

        StartCoroutine(DealDamageOverTime(damagePerSecond, totalSeconds));
    }

    public void Initialize(float damagePerSecond, Color NewColor)
    {
        //Debug.Log("DOT initialized! dealing " + damagePerSecond + " every second.");

        Start();

        overrideColor = NewColor;
        characterBehaviour.colorOverride = NewColor;

        StartCoroutine(DealDamageOverTimeForever(damagePerSecond));
    }

    /// <summary>
    /// deals damage ever 1/2 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator DealDamageOverTimeForever(float damagePerSecond)
    {
        while(this != null)
        {
            //Debug.Log("Damaging!");
            spriteRenderer.color = overrideColor;

            characterBehaviour.TakeDamage(damagePerSecond / 2, false);

            yield return new WaitForSeconds(damageOverTimeInterval);
        }
    }

    /// <summary>
    /// deals damage ever 1/2 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator DealDamageOverTime(float damagePerSecond, float totalSeconds)
    {
        float iterations = totalSeconds / damageOverTimeInterval;

        for(int i=0; i< iterations; i++) 
        {
            //Debug.Log("Damaging!");
            spriteRenderer.color = overrideColor;

            characterBehaviour.TakeDamage(damagePerSecond / 2, false);

            yield return new WaitForSeconds(damageOverTimeInterval);
        }

        Stop();
    }

    private void OnDestroy()
    {
        ResetCharacterColor();
    }

    /// <summary>
    /// Sets characters color back to original color if its not taking damage over time
    /// </summary>
    private void ResetCharacterColor()
    {
        //Debug.Log("Destroying DOT after " + (Time.time - t) + " seconds");

        DamageOverTime[] test = GetComponents<DamageOverTime>();

        bool multipleDOTs = test.Length > 1;

        if (multipleDOTs)
            return;

        spriteRenderer.color = characterBehaviour.originalColor;
        characterBehaviour.colorOverride = characterBehaviour.originalColor;

    }
}
