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

    private void Start()
    {
        shadowTargetScaleTarget = Shadow.transform.localScale;
        Shadow.transform.localScale = Vector3.zero;

        StartCoroutine(InitializeBomb());

        if(RotateBomb)
            StartCoroutine(RotateBombCoroutine());
    }

    /// <summary>
    /// Stage 1.
    /// Lerps the bombs opacity 0 > 1 and moves it up 1 unit.
    /// </summary>
    /// <returns></returns>
    public IEnumerator InitializeBomb()
    {
        //magic numbers
        float opacityFrames = 50;
        float unitsUp = 1;

        float t = 0;
        Vector3 newPos;

        SpriteRenderer bombSprite = Bomb.GetComponent<SpriteRenderer>();
        Color bombColor = bombSprite.color;

        bombColor.a = 0f;
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
            newPos = Bomb.transform.position;
            newPos.y += unitsUp / opacityFrames;
            Bomb.transform.position = newPos;

            yield return new WaitForSeconds(SuspendedTime / opacityFrames);
        }

        StartCoroutine(DropBomb());
    }

    /// <summary>
    /// Stage 2.
    /// Makes bomb fall
    /// </summary>
    /// <returns></returns>
    public IEnumerator DropBomb()
    {
        float t = 0;

        Vector3 bombStart = Bomb.transform.position;
        Vector3 bombTarget = Shadow.transform.position;

        SpriteRenderer shadowSprite = Shadow.GetComponent<SpriteRenderer>();
        float targetShadowOpacity = shadowSprite.color.a;

        //drop bomb. expand shadow. expand opacity
        while (t < 1)
        {
            //i feel like a baby calf and the lerp function is the udder of which i derive my nutrients
            t += 1 / FallingFrames;
            float tScaled = Mathf.Pow(t, FallingAcceleration);

            Bomb.transform.position = Vector3.Lerp(bombStart, bombTarget, tScaled);
            Shadow.transform.localScale = Vector3.Lerp(Vector3.zero, shadowTargetScaleTarget, tScaled);

            Color shadowColor = shadowSprite.color;
            shadowColor.a = Mathf.Lerp(0,targetShadowOpacity,tScaled);
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

