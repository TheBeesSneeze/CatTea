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
    public bool TakeKnockback = true;

    [Header("Don't touch these in editor")]
    public int HealthPoints;
    public float Speed;
    public float KnockbackForce;
    

    //lame stuff
    protected Rigidbody2D myRigidbody2D;

    protected virtual void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();

        SetStatsToDefaults();
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
    /// Takes damage without any knockback
    /// </summary>
    /// <param name="damage">Amt of damage taken</param>
    /// <returns>true if character died</returns>
    public virtual bool TakeDamage(int damage)
    {
        HealthPoints -= damage;

        if (HealthPoints <= 0)
        {
            Die();
            return true;
        }
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
    /// <param name="Force">how much to multiply knockback by</param>
    public virtual void KnockBack(GameObject target, Vector3 damageSourcePosition, float Force)
    {
        Vector2 positionDifference = target.transform.position - damageSourcePosition;
        positionDifference.Normalize();

        if (myRigidbody2D != null)
            myRigidbody2D.AddForce(positionDifference * Force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Code for when the characters health reaches below 0
    /// </summary>
    public virtual void Die()
    {
        Debug.LogWarning("Override this function");
    }

    public virtual void SetStatsToDefaults()
    {
        Debug.LogWarning("Override me!");
    }
}