/*******************************************************************************
* File Name :         BossBehaviour.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/18/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : CharacterBehaviour
{
    public float StartHealth;
    public float MoveUnitsPerSecond;
    //public int CurrentHealth;
    

    [HideInInspector] public BossRoom MyRoom;
    private Animator animator;

    private float shakeAmount = 0.2f;
    private float deathAnimationSeconds = 2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = CharacterSprite.GetComponent<Animator>();
        MaxHealthPoints = StartHealth; //yeah

        SetHealth(StartHealth);

        base.Start();
    }

    public override bool TakeDamage(float damage)
    {
        return TakeDamage(damage,true);
    }

    public override bool TakeDamage(float damage, bool onDamageEvent)
    {
        if (HealthPoints - damage > 0 && onDamageEvent)
            GameEvents.Instance.OnEnemyDamage(this);

        return base.TakeDamage(damage);
    }

    public override void SetHealth(float value)
    {
        base.SetHealth(value);

        if (animator == null)
        {
            Debug.LogWarning(gameObject.name + " does not have an animator");
            return;
        }

        animator.SetFloat("Health", value);
    }

    public override void Die()
    {
        base.Die();

        Debug.Log("OH NO IM DYING!");

        GameEvents.Instance.OnEnemyDeath(this.transform.position);

        Debug.Log("Boss die!");
        
        //temp code hopefully
        StopAllAttacks();
        //StartCoroutine(DeathAnimation());
    }

    /// <summary>
    /// called at end of animation
    /// </summary>
    public virtual void DieForReal()
    {
        MyRoom.OnBossDeath();
    }

    public void StopAllAttacks()
    {
        BossAttackType[] allAttacks = GetComponents<BossAttackType>();

        foreach(BossAttackType attack in allAttacks)
        {
            attack.StopAttack();
        }
    }

    /// <summary>
    /// *Vibrates violently*
    /// *dies*
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator DeathAnimation()
    {
        Vector2 centerPosition = transform.position;

        float t = 0;
        while(t< deathAnimationSeconds)
        {
            t += Time.deltaTime;

            float maxShakeAmount = Mathf.Lerp(0, shakeAmount, t / deathAnimationSeconds);

            float x = Random.Range(-maxShakeAmount, maxShakeAmount);
            float y = Random.Range(-maxShakeAmount, maxShakeAmount);

            transform.localPosition = centerPosition + new Vector2(x, y);

            yield return null;
        }
        

        Destroy(this.gameObject);
    }
}
