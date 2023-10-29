/*******************************************************************************
* File Name :         OozeAttack.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/28/2023
*
* Brief Description : Randomizes size and opacity
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OozeAttack : AttackType
{
    
    [Header("Ooze settings")]
    public Color OozedEnemyColor;// = new Color(0.3f, 0.4f, 0.8f);

    private float t; // 1 >= t >= 0     (puddle destroyed when t = 0)
    private Vector3 startScale;

    //private OozeUpgrade oozeUpgrade;


    protected override void Start()
    {
        //oozeUpgrade = GameObject.FindObjectOfType<OozeUpgrade>();

        base.Start();  
        RandomizeSize();
        RandomizeOpacity();

        StartCoroutine(ShrinkOoze());
    }

    private void RandomizeSize()
    {
        float r = Random.Range(0.5f, 1.5f);

        startScale = transform.localScale * r;
        transform.localScale = startScale;
    }

    private void RandomizeOpacity()
    {
        float r = Random.Range(0.5f, 1.5f);

        float a = 0.5f * r;

        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        Color c = sprite.color;

        sprite.color = new Color(c.r, c.g, c.b, a);
    }

    private IEnumerator ShrinkOoze()
    {
        t = 1; // 0<= t <= 1
        while (t > 0)
        {
            t -= Time.deltaTime/ DestroyAttackAfterSeconds;
            transform.localScale = startScale * t;

            yield return null;
        }

        Destroy(gameObject);
    }

    protected override void OnBossCollision(Collider2D collision)
    {
        Debug.Log("boss ooze");
        DamageOverTime DOT = collision.AddComponent<DamageOverTime>();
        DOT.Initialize(Damage, DestroyAttackAfterSeconds * t, OozedEnemyColor);
    }

    protected override void OnEnemyCollision(Collider2D collision)
    {
        Debug.Log("enemy ooze");
        DamageOverTime DOT = collision.AddComponent<DamageOverTime>();
        DOT.Initialize(Damage, DestroyAttackAfterSeconds * t, OozedEnemyColor);
    }
    
    /// <summary>
    /// mmmmm yeah ill have the nothing code with the override sauce
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DestroyAfterSeconds()
    {
        yield return null;
    }
}
