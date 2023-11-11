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
    protected PlayerBehaviour playerBehaviour;

    private float shakeAmount = 0.2f;
    private float deathAnimationSeconds = 2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        MaxHealthPoints = StartHealth; //yeah
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

    public override void Die()
    {
        base.Die();

        GameEvents.Instance.OnEnemyDeath(this.transform.position);

        Debug.Log("Boss die!");
        MyRoom.OnBossDeath();

        //temp code hopefully
        StopAllAttacks();
        StartCoroutine(DeathAnimation());
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
