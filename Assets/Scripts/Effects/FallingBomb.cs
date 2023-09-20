/*******************************************************************************
* File Name :         FallingBomb.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/17/2023
*
* Brief Description : Has a bomb gameobject which falls. The shadow underneath
* slowly expands. Lets out a explosion when its done.
* 
* Bombs automatically become slightly transparent to let the player know they're
* in the foreground
* 
* The attack is on the explosion object!
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class FallingBomb : MonoBehaviour
{
    public GameObject Bomb;
    public GameObject Shadow;
    public GameObject ExplosionPrefab;

    public bool RotateBomb;

    [Tooltip("Time Bomb Spends in air")]
    public float SuspendedTime;

    [Tooltip("Time for bomb to fall")]
    public float FallingTime;
    [Tooltip("Time the explosion lasts for")]
    public float ExplosionTime;

    [Tooltip("How many times the bombs position will be calculated ofer FallingTIme")]
    public float FallingFrames;

    [Tooltip("Idk. you know what acceleration is.")]
    public float FallingAcceleration;

    private Vector3 shadowTargetScaleTarget;
    private float targetShadowOpacity;
    private SpriteRenderer shadowSprite;

    private float initialShadowOpacity = 0.3f;

    private void Start()
    {
        shadowTargetScaleTarget = Shadow.transform.localScale;
        Shadow.transform.localScale = Shadow.transform.localScale / 2;

        shadowSprite = Shadow.GetComponent<SpriteRenderer>();
        targetShadowOpacity = shadowSprite.color.a;
        shadowSprite.color = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, initialShadowOpacity);

        StartCoroutine(InitializeBomb());

        if(RotateBomb)
            StartCoroutine(RotateBombCoroutine());
    }

    /// <summary>
    /// Stage 1.
    /// Lerps the bombs opacity 0 > 1 and moves it up 1 unit.
    /// Slightly expands shadow
    /// </summary>
    /// <returns></returns>
    public IEnumerator InitializeBomb()
    {
        //magic numbers
        float opacityFrames = 50;
        float unitsUp = 1;

        float t = 0;
        Vector3 newBombPosition;

        Vector3 targetShadowSize = Shadow.transform.localScale;
        Color targetShadowColor = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, initialShadowOpacity);

        SpriteRenderer bombSprite = Bomb.GetComponent<SpriteRenderer>();
        Color bombColor = bombSprite.color;

        bombColor.a = 0;
        bombSprite.color = bombColor;

        //make bomb more clear over waiting time

        //float opacityInterval = (FallingTime * SuspendedTime) / FallingFrames; //had to bust out the middle school algebra for this one //it didnt work
        while (t < 1)
        {
            t += 1.5f / opacityFrames;
            float tScaled = Mathf.Pow(t, FallingAcceleration);

            //make visible
            bombColor.a = Mathf.Lerp(0, 1, tScaled);
            bombSprite.color = bombColor;

            //move up slightly
            newBombPosition = Bomb.transform.position;
            newBombPosition.y += unitsUp / opacityFrames;
            Bomb.transform.position = newBombPosition;

            //expand shadow slightly
            Shadow.transform.localScale = Vector3.Lerp(Vector3.zero, targetShadowSize, t);
            shadowSprite.color = Color.Lerp(shadowSprite.color, targetShadowColor, t) ; //guys i am fucking losing it

            yield return new WaitForSeconds(SuspendedTime / opacityFrames);
        }

        StartCoroutine(DropBomb());
    }

    /// <summary>
    /// Stage 2.
    /// Makes bomb fall.
    /// Stops being a child.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DropBomb()
    {
        transform.SetParent(null);

        float t = 0;

        Vector3 bombStart = Bomb.transform.position;
        Vector3 bombTarget = Shadow.transform.position;

        Vector3 ShadowStartSize = Shadow.transform.localScale;

        float shadowStartOpacity = shadowSprite.color.a;

        //drop bomb. expand shadow. expand opacity
        while (t < 1)
        {
            //i feel like a baby calf and the lerp function is the udder of which i derive my nutrients
            t += 1 / FallingFrames;
            float tScaled = Mathf.Pow(t, FallingAcceleration);

            Bomb.transform.position = Vector3.Lerp(bombStart, bombTarget, tScaled);
            Shadow.transform.localScale = Vector3.Lerp(ShadowStartSize, shadowTargetScaleTarget, tScaled);

            Color shadowColor = shadowSprite.color;
            shadowColor.a = Mathf.Lerp(shadowStartOpacity, targetShadowOpacity,tScaled);
            shadowSprite.color = shadowColor;

            yield return new WaitForSeconds(FallingTime / FallingFrames);
        }

        StartCoroutine(Explode());
    }

    

    public IEnumerator RotateBombCoroutine()
    {
        float increment = FallingTime / FallingFrames;

        while (Bomb != null)
        {
            float angle = Bomb.transform.eulerAngles.z;
            angle += increment*-25;

            Bomb.transform.eulerAngles = new Vector3(0,0,angle);

            yield return new WaitForSeconds(increment);
        }
    }

    /// <summary>
    /// Destroy bomb and shadow.
    /// Instantiate explosion.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Explode()
    {
        GameObject explosion = Instantiate(ExplosionPrefab, Shadow.transform.position, Quaternion.identity);

        Destroy(Bomb);
        Destroy(Shadow);

        yield return new WaitForSeconds(ExplosionTime);

        Destroy(explosion);
    }
}

