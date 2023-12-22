/*******************************************************************************
* File Name :         BossHealthBar.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/23/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;

    private float MaxHealth;
    public Slider healthBar;
    [HideInInspector] public BossBehaviour Boss;

    private bool animating;

    private Coroutine animateCoroutine;

    private float secondsPerHealthPointStart = 0.2f;
    private float secondsPerHealthPoint = 0.1f;

    public void ActivateHealthBar(BossBehaviour boss)
    {
        Boss = boss;

        healthBar.gameObject.SetActive(true);

        StartCoroutine(AnimateHealthbarStart());
    }

    private IEnumerator AnimateHealthbarStart()
    {
        animating = true;

        float projectedValue = 0;

        while(projectedValue < Boss.HealthPoints)
        {
            projectedValue += Time.deltaTime / secondsPerHealthPointStart;

            MaxHealth = Boss.MaxHealthPoints;
            healthBar.maxValue = MaxHealth;
            healthBar.value = projectedValue;

            yield return null;
        }

        animating = false;

        UpdateHealth();

        animateCoroutine = StartCoroutine(AnimateHealthBar());
    }

    private IEnumerator AnimateHealthBar()
    {
        float displayHealth = healthBar.value;

        while (healthBar.gameObject.activeSelf)
        {
            if(Boss.HealthPoints <  displayHealth)
            {
                displayHealth -= Time.deltaTime/secondsPerHealthPoint;
                healthBar.value = displayHealth;
            }

            if (Boss.HealthPoints > displayHealth)
            {
                displayHealth += Time.deltaTime / secondsPerHealthPoint;
                healthBar.value = displayHealth;
            }

            if (Boss.HealthPoints <= 0)
                healthBar.gameObject.SetActive(false);

            yield return null;
        }

        animateCoroutine = null;
    }

    public void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    public void UpdateHealth()
    {
        if (Boss == null)
            return;

        MaxHealth = Boss.MaxHealthPoints;

        if (animating)
            return;

        healthBar.maxValue = MaxHealth;
        healthBar.value = Boss.HealthPoints;
    }

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
            Instance = this;
    }
}
