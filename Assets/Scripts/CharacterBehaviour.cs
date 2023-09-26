/*******************************************************************************
* File Name :         CharacterBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Code that is shared between players and enemies. Does
* not relate to talking NPCs.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField]private int _healthPoints;
    public int HealthPoints
    {
        get {  return _healthPoints; }
        set { SetHealth(value); }
    }

    //[Tooltip("Ignores damage if true")]
    [HideInInspector] public bool Invincible;

    [HideInInspector] public int MaxHealthPoints;
    [HideInInspector] public float Speed;
    [HideInInspector] public float KnockbackForce;
    [HideInInspector] public bool TakeKnockback = true;

    //lame stuff
    protected Rigidbody2D myRigidbody2D;
    protected SpriteRenderer mySpriteRenderer;

    private Coroutine hitAnimationCoroutine;
    private Coroutine invincibleCoroutine;

    protected virtual void Start()
    {
        _healthPoints = MaxHealthPoints;

        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        SetStatsToDefaults();
    }

    /// <summary>
    /// Takes damage without any knockback.
    /// Checks for invincibility
    /// </summary>
    /// <param name="damage">Amt of damage taken</param>
    /// <returns>true if character died</returns>
    public virtual bool TakeDamage(int damage)
    {
        if (Invincible)
            return false;

        if (hitAnimationCoroutine != null)
            StopCoroutine(hitAnimationCoroutine);

        hitAnimationCoroutine = StartCoroutine(HitAnimation());

        HealthPoints -= damage;

        if (HealthPoints <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Decreases the characters health. Knocks back the character
    /// </summary>
    /// <param name="damage">Amt of damage taken</param>
    /// <param name="damageSourcePosition">Ideally the players transform</param>
    /// <returns>true if character died</returns>
    public virtual bool TakeDamage(int Damage, Vector3 DamageSourcePosition)
    {
        return TakeDamage(Damage, DamageSourcePosition, KnockbackForce);
    }

    /// <summary>
    /// Decreases the characters health. Knocks back the character
    /// </summary>
    /// <param name="damage">Amt of damage taken</param>
    /// <param name="damageSourcePosition">Ideally the players transform</param>
    /// <param name="KnockBackForce"></param>
    /// <returns>true if character died</returns>
    public virtual bool TakeDamage(int Damage, Vector3 DamageSourcePosition, float KnockBackForce)
    {
        if (TakeDamage(Damage))
            return true;

        if (TakeKnockback)
            KnockBack(this.gameObject, DamageSourcePosition, KnockBackForce);

        return false;
    }

    

    /// <summary>
    /// Knocks back target away from damageSourcePosition
    /// </summary>
    /// <param name="target">who is getting knockedback</param>
    /// <param name="damageSourcePosition">where they are getting knocked back from</param>
    public virtual void KnockBack(GameObject target, Vector3 damageSourcePosition)
    {
        KnockBack(target, damageSourcePosition, KnockbackForce);
    }

    /// <summary>
    /// Knockback override to include knockbackforce override
    /// </summary>
    /// <param name="target">who is getting knockedback</param>
    /// <param name="damageSourcePosition">where they are getting knocked back from</param>
    /// <param name="force">how much to multiply knockback by</param>
    public virtual void KnockBack(GameObject target, Vector3 damageSourcePosition, float force)
    {
        Vector2 positionDifference = target.transform.position - damageSourcePosition;
        positionDifference.Normalize();

        if (myRigidbody2D != null)
            myRigidbody2D.AddForce(positionDifference * force, ForceMode2D.Impulse);
    }

    public virtual IEnumerator HitAnimation()
    {
        mySpriteRenderer.color = new Color(1, 0.3f, 0.3f);

        yield return new WaitForSeconds(0.1f);

        mySpriteRenderer.color = new Color(1, 1,1);
    }

    /// <summary>
    /// Code for when the characters health reaches below 0
    /// </summary>
    public virtual void Die()
    {
        Debug.LogWarning("Override this function");
    }

    public virtual void SetHealth(int value)
    {
        _healthPoints = value;
    }

    public virtual void SetStatsToDefaults()
    {
        //Debug.LogWarning("Override me!");
    }

    /// <summary>
    /// Makes the character invincible over invincibilitySeconds
    /// </summary>
    /// <param name="invincibilitySeconds">Length of invincibility</param>
    public virtual void BecomeInvincible(float invincibilitySeconds)
    {
        Invincible = true;

        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);

        StartCoroutine(BecomeVincible(invincibilitySeconds));
    }

    private IEnumerator BecomeVincible(float invincibilitySeconds)
    {
        yield return new WaitForSeconds(invincibilitySeconds);
        Invincible = false;
    }
}