/*******************************************************************************
* File Name :         LightColorCycler.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/10/23
*
* Brief Description : Small visual effect which smoothly changes the color of an attached 
* Light2D component. Cycles between an array of colors.
* 
* Can be accessed by other scripts to change a lights color!
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightColorCycler : MonoBehaviour
{
    [Tooltip("Displays these colors in order")]
    public Color[] Colors;
    public float SecondsBetweenEachColor;
    public bool CycleColors = true;

    private Light2D _light;

    private int colorIndex;
    private Coroutine transitionCoroutine;

    public void TransitionColor(Color color)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        StartCoroutine(LerpColor(color));
    }
    /// <summary>
    /// Change the light2ds color over n seconds
    /// </summary>
    public void TransitionColor(Color color, float seconds)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        SecondsBetweenEachColor = seconds;

        StartCoroutine(LerpColor(color));
    }

    private void Start()
    {
        _light = GetComponent<Light2D>();

        if(CycleColors)
        {
            _light.color = Colors[colorIndex];

            colorIndex = (colorIndex+1) % Colors.Length;

            StartCoroutine(LerpColor(Colors[colorIndex]));
        }
        
    }

    private IEnumerator LerpColor(Color color)
    {
        Color oldColor = _light.color;
        color.a = 1;

        float t = 0;

        while(t< SecondsBetweenEachColor) 
        {
            t+= Time.deltaTime;
            float tScaled = Mathf.Sin( (t*Mathf.PI)/(SecondsBetweenEachColor*2) );
            _light.color = Color.Lerp(oldColor, color, tScaled);
            yield return null;
        }

        if(CycleColors)
        {
            colorIndex = (colorIndex+1) % Colors.Length;
            StartCoroutine(LerpColor(Colors[colorIndex]));
        }

        transitionCoroutine = null;
    }
    
}
