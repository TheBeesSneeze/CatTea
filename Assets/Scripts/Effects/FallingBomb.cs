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

    [Tooltip("Idk. you know what acceleration is.")]
    public float FallingAcceleration;

    private Vector3 shadowTargetScaleTarget;
    private float targetShadowOpacity;
    private SpriteRenderer shadowSprite;
    private LineRenderer lineRenderer;

    private float initialShadowOpacity = 0.3f;

    private void Start()
    {
        shadowTargetScaleTarget = Shadow.transform.localScale;
        Shadow.transform.localScale = Shadow.transform.localScale / 2;

        shadowSprite = Shadow.GetComponent<SpriteRenderer>();
        targetShadowOpacity = shadowSprite.color.a;
        shadowSprite.color = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, initialShadowOpacity);

        lineRenderer = GetComponent<LineRenderer>();

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
        float unitsUp = 1;

        float time = 0;
        Vector3 newBombPosition;

        Vector3 targetShadowSize = Shadow.transform.localScale;
        Color targetShadowColor = new Color(shadowSprite.color.r, shadowSprite.color.g, shadowSprite.color.b, initialShadowOpacity);

        SpriteRenderer bombSprite = Bomb.GetComponent<SpriteRenderer>();
        Color bombColor = bombSprite.color;

        bombColor.a = 0;
        bombSprite.color = bombColor;

        //make bomb more clear over waiting time

        //float opacityInterval = (FallingTime * SuspendedTime) / FallingFrames; //had to bust out the middle school algebra for this one //it didnt work
        while (time < SuspendedTime)
        {
            time += Time.deltaTime;
            float t = time / SuspendedTime;
            float tScaled = Mathf.Pow(t, FallingAcceleration);

            //make visible
            bombColor.a = Mathf.Lerp(0, 1, tScaled);
            bombSprite.color = bombColor;

            //move up slightly
            newBombPosition = Bomb.transform.position;
            newBombPosition.y += unitsUp * Time.deltaTime;
            Bomb.transform.position = newBombPosition;

            //expand shadow slightly
            Shadow.transform.localScale = Vector3.Lerp(Vector3.zero, targetShadowSize, t);
            shadowSprite.color = Color.Lerp(shadowSprite.color, targetShadowColor, t) ; //guys i am fucking losing it

            //draw line
            UpdateLineRenderer();

            yield return null;
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

        

        Vector3 bombStart = Bomb.transform.position;
        Vector3 bombTarget = Shadow.transform.position;

        Vector3 ShadowStartSize = Shadow.transform.localScale;

        float shadowStartOpacity = shadowSprite.color.a;

        float time = 0;
        //drop bomb. expand shadow. expand opacity
        while (time < FallingTime)
        {
            //i feel like a baby calf and the lerp function is the udder of which i derive my nutrients
            time += Time.deltaTime;
            float t = time / FallingTime;
            float tScaled = Mathf.Pow(t, FallingAcceleration);

            Bomb.transform.position = Vector3.Lerp(bombStart, bombTarget, tScaled);
            Shadow.transform.localScale = Vector3.Lerp(ShadowStartSize, shadowTargetScaleTarget, tScaled);

            Color shadowColor = shadowSprite.color;
            shadowColor.a = Mathf.Lerp(shadowStartOpacity, targetShadowOpacity,tScaled);
            shadowSprite.color = shadowColor;

            UpdateLineRenderer();

            yield return null;
        }

        StartCoroutine(Explode());
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, Bomb.transform.position);
        lineRenderer.SetPosition(1, Shadow.transform.position);
    }

    public IEnumerator RotateBombCoroutine()
    {
        while (Bomb != null)
        {
            float angle = Bomb.transform.eulerAngles.z;

            float increment = FallingTime * Time.deltaTime;
            angle += increment * -25;

            Bomb.transform.eulerAngles = new Vector3(0,0,angle);

            yield return null;
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

