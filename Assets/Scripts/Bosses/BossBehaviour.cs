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

    [Tooltip("Wont drop an upgrade if the gun is supposed to be dropped instead")]
    public bool DropUpgradeOnDeath = true;
    //public int CurrentHealth;

    [HideInInspector]public bool DialogueEnded;

    [HideInInspector] public BossRoom MyRoom;

    private float deathAnimationSeconds = 2f;

    protected Vector2 startPosition;

    // Start is called before the first frame update
    protected override void Start()
    {
        MyAnimator = CharacterSprite.GetComponent<Animator>();
        MaxHealthPoints = StartHealth; //yeah

        SetHealth(StartHealth);

        base.Start();

        
    }

    /// <summary>
    /// Runs when the boss is ready to be fought
    /// </summary>
    public virtual void Initialize()
    {
        BossHealthBar.Instance.ActivateHealthBar(this);
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

        if (MyAnimator == null)
        {
            Debug.LogWarning(gameObject.name + " does not have an animator");
            return;
        }

        MyAnimator.SetFloat("Health", value);

        BossHealthBar.Instance.UpdateHealth();
    }

    public override void Die()
    {
        Debug.Log("im dying!");
        base.Die();

        GameEvents.Instance.OnEnemyDeath(this.transform.position);
        
        StopAllAttacks();

        StartCoroutine(DeathDelay());
    }

    protected IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathAnimationSeconds);
        DieForReal();
    }

    /// <summary>
    /// called at end of animation
    /// </summary>
    public virtual void DieForReal()
    {
        if (DropUpgradeOnDeath)
            Instantiate(UniversalVariables.Instance.UpgradeCollectionPrefab, transform.position, Quaternion.identity);

        MyRoom.OnBossDeath();
        Destroy(gameObject);

        BossHealthBar.Instance.HideHealthBar();
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
    /*
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
    */
}
