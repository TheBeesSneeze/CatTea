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
using Unity.VisualScripting;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public GameObject CharacterSprite;

    [Header("Sounds")]
    public AudioSource DamageSound;
    public AudioClip DeathClip;

    [SerializeField]private float _healthPoints;
    public float HealthPoints
    {
        get {  return _healthPoints; }
        set { SetHealth(value); }
    }

    //[Tooltip("Ignores damage if true")]
    [HideInInspector] public bool Invincible;
    private bool died;

    [HideInInspector] public float MaxHealthPoints;
    [HideInInspector] public float Speed;
    [HideInInspector] public float KnockbackForce;
    [HideInInspector] public bool TakeKnockback = true;

    //magic numbers
    protected bool capHPAtMax = true;

    [HideInInspector] public Animator MyAnimator;

    //lame stuff
    protected Rigidbody2D myRigidbody2D;
    protected SpriteRenderer spriteRenderer;

    private Coroutine invincibleCoroutine;

    [HideInInspector] public Color originalColor;
    [HideInInspector] public Color colorOverride;

    //[Header("Debug")]
    [HideInInspector] public List<GameObject> AttacksSpawned; //most of the gameobjects in this list will be null
    
    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        if (CharacterSprite == null)
            CharacterSprite = gameObject;

        MyAnimator = CharacterSprite.GetComponent<Animator>();

        _healthPoints = MaxHealthPoints;

        myRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = CharacterSprite.GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        colorOverride = originalColor;

        SetStatsToDefaults();
    }

    /// <summary>
    /// Takes damage without any knockback.
    /// Checks for invincibility
    /// </summary>
    /// <param name="damage">Amt of damage taken</param>
    /// <returns>true if character died</returns>
    public virtual bool TakeDamage(float damage)
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("TakeDamage");

        if (Invincible)
            return false;

        HealthPoints -= damage;

        if (HealthPoints <= 0 && !died)
        {
            Die();
            return true;
        }

        PlayDamageSound();

        return false;
    }

    public virtual bool TakeDamage(float damage, bool onDamageEvent)
    {
        Debug.LogWarning("Override this function");
        return TakeDamage(damage);
    }

    /// <summary>
    /// Decreases the characters health. Knocks back the character
    /// </summary>
    /// <param name="damage">Amt of damage taken</param>
    /// <param name="damageSourcePosition">Ideally the players transform</param>
    /// <returns>true if character died</returns>
    public virtual bool TakeDamage(float Damage, Vector3 DamageSourcePosition)
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
    public virtual bool TakeDamage(float Damage, Vector3 DamageSourcePosition, float KnockBackForce)
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
        {
            Debug.Log(positionDifference);
            myRigidbody2D.AddForce(positionDifference * force, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Code for when the characters health reaches below 0
    /// </summary>
    public virtual void Die()
    {
        died = true;
        PlayDeathSound();
        GameManager.Instance.DestroyAllObjectsInList(AttacksSpawned);
    }

    public virtual void SetHealth(float value)
    {
        //Debug.Log(MaxHealthPoints + " max");
        if(capHPAtMax)
        {
            value = Mathf.Clamp(value, value, MaxHealthPoints);
        }

        _healthPoints = value;
    }

    public virtual void SetStatsToDefaults()
    {
        //Debug.LogWarning("Override me!");
    }
    
    public virtual void PlayDamageSound()
    {
        if (DamageSound == null)
        {
            Debug.LogWarning(gameObject.name + " does not have a damage audio source");
            return;
        }

        if (DamageSound.clip == null)
        {
            Debug.LogWarning(gameObject.name + " does has an audio source, but no damage audio clip");
            return;
        }

        float randomPitch = Random.Range(0.9f, 1.1f);

        DamageSound.pitch = randomPitch;
        DamageSound.Play();
    }

    public virtual void PlayDeathSound()
    {
        if(DeathClip == null)
        {
            Debug.LogWarning(gameObject.name + " does not have a death audio clip");
            return;
        }

        AudioSource.PlayClipAtPoint(DeathClip, transform.position, SaveDataManager.Instance.SettingsData.SoundVolume);
    }

    public virtual void BecomeInvincible(float invincibilitySeconds, bool becomeClear)
    {
        if(becomeClear)
            StartCoroutine(ScaleOpacity(0.5f, invincibilitySeconds));

        Invincible = true;

        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);

        StartCoroutine(BecomeVincible(invincibilitySeconds, becomeClear));
    }

    /// <summary>
    /// Makes the character invincible over invincibilitySeconds
    /// </summary>
    /// <param name="invincibilitySeconds">Length of invincibility</param>
    public virtual void BecomeInvincible(float invincibilitySeconds)
    {
        BecomeInvincible(invincibilitySeconds, false);
    }

    private IEnumerator BecomeVincible(float invincibilitySeconds, bool becomeClear)
    {
        yield return new WaitForSeconds(invincibilitySeconds);
        Invincible = false;

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

        if(becomeClear)
            StartCoroutine(ScaleOpacity(1f, invincibilitySeconds*2));
    }

    private IEnumerator ScaleOpacity(float targetOpacity,float seconds)
    {
        float t = 0;
        Color c = spriteRenderer.color;

        while(t< seconds)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1 - targetOpacity, targetOpacity, t / seconds);

            spriteRenderer.color = c;

            yield return null;
        }
    }

    /*
    /// <summary>
    /// this could totally be done with the MyAnimator but i like code >:)
    /// </summary>
    private IEnumerator HitBounceAnimation()
    {
        float defaultScale = transform.localScale.y;

        float time = 0;
        while(time< damageColorChangeSeconds)
        {
            time+= Time.deltaTime;
            float t = time / damageColorChangeSeconds;

            float p = Mathf.Sin(2 * Mathf.PI * t);

            float y = defaultScale + (defaultScale * damageBouncePercent * p); ;
            transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
            
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);

        //bounceAnimationCoroutine = null;
    }
    */

    /*
    public virtual IEnumerator HitAnimation()
    {

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //if(bounceAnimationCoroutine == null)
        //    bounceAnimationCoroutine = StartCoroutine(HitBounceAnimation());

        spriteRenderer.color = new Color(1, 0.3f, 0.3f);

        yield return new WaitForSeconds(damageColorChangeSeconds);

        spriteRenderer.color = colorOverride;
    }
    */

    /*
    /// <summary>
    /// Makes the character start to take damage over duration.
    /// Can be stacked by calling multiple times.
    /// </summary>
    /// <param name="totalDamage">how much damage will be taken after the effect ends</param>
    /// <param name="duration">how many seconds the character takes damage for</param>
    /// <param name="damageInterval">how often the character takes damage in a second</param>
    /// <returns></returns>
    public virtual void TakeDamageOverTime(float totalDamage, float duration, float damageInterval)
    {
        //i think its weird when external scripts start coroutines on other scripts. sorry guys
      
        StartCoroutine(DamageOverTime(totalDamage, duration, damageInterval));
    }

    private IEnumerator DamageOverTime(float totalDamage, float duration, float damageInterval)
    {
        Color oldColor = spriteRenderer.color;

        spriteRenderer.color = new Color(0.25f, 0.7f, 0.9f);

        int iterations = (int)(duration / damageInterval);
        int i = 0;

        while (i < iterations)
        {
            TakeDamage(totalDamage/((float)iterations));

            yield return new WaitForSeconds(damageInterval);
        }

        spriteRenderer.color = oldColor;
    }
    */
}