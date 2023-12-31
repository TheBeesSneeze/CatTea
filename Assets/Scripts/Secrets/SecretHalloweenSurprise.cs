using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*******************************************************************************
* File Name :         SecretHalloweenSurprise.cs
* Author(s) :         THE PUMPKIN KING
* Creation Date :     10/31/2023
*
* Brief Description : MWAHAHAHAHAHA HAPPY HALLOWEEN!!!!
*****************************************************************************/

public class SecretHalloweenSurprise : MonoBehaviour
{
    public Sprite SPOOKY_PUMPKIN;
    public Sprite MERRY_CHRISTMAS;

    // Start is NOT called before the first frame update! MWAHAHAHAHA
    void Start()
    {
        DateTime halloween = new DateTime(DateTime.Now.Year, 10, 31);
        if (DateTime.Today == halloween)
        {
            StartCoroutine(HALLOWEEN_SURPRISE(SPOOKY_PUMPKIN));
        }

        DateTime christmas = new DateTime(DateTime.Now.Year, 12, 25);
        if (DateTime.Today == christmas)
        {
            StartCoroutine(HALLOWEEN_SURPRISE(MERRY_CHRISTMAS));
        }
    }

    /// <summary>
    /// MWAHAHAHAHA HAPPY HALLOWEEN
    /// </summary>
    private IEnumerator HALLOWEEN_SURPRISE(Sprite HOLLIDAY_SURPRISE)
    {

        yield return new WaitForSeconds(1);

        while (true)
        {
            Animator[] NO_MORE_ANIMATORS = GameObject.FindObjectsOfType<Animator>();

            foreach (Animator VICTIM in NO_MORE_ANIMATORS)
            {
                VICTIM.enabled = false;
            }

            SpriteRenderer[] EVERY_SPRITE_IN_THE_GAME = GameObject.FindObjectsOfType<SpriteRenderer>();

            foreach (SpriteRenderer VICTIM in EVERY_SPRITE_IN_THE_GAME)
            {
                VICTIM.sprite = HOLLIDAY_SURPRISE;
            }

            Image[] EVERY_UI_SPRITE_IN_THE_GAME = GameObject.FindObjectsOfType<Image>();

            foreach (Image VICTIM in EVERY_UI_SPRITE_IN_THE_GAME)
            {
                VICTIM.sprite = HOLLIDAY_SURPRISE;
            }

            yield return null;
        }
    }
}
