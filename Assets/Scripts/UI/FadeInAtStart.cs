/*******************************************************************************
* File Name :         FadeInAtStart.cs
* Author(s) :         Toby Schamberger
* Creation Date :     12/2/2023
*
* Brief Description : hi zach dont look at the creation date.
* literally just a function to destroy the gameobject, to be used in the animator. (couldnt figure it out)
* because man i dont know what im doing
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAtStart : MonoBehaviour
{

    public float SecondsBeforeFading;
    public float SecondsToFadeIn;

    private Image fadeImage;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();

        Color color = fadeImage.color;
        color.a = 1;

        fadeImage.color = color;
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        Color oldColor = fadeImage.color;

        Color targetColor = oldColor;
        targetColor.a = 0;

        yield return new WaitForSeconds(SecondsBeforeFading);

        float time = 0;
        while (time < SecondsToFadeIn)
        {
            time += Time.deltaTime;
            float t = time / SecondsToFadeIn;

            Color newColor = Color.Lerp(oldColor, targetColor, t);

            fadeImage.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
