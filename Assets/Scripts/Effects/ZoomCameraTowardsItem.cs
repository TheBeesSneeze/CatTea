/*******************************************************************************
* File Name :         ZoomCameraTowardsItem.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/10/2023
*
* Brief Description : Zooms the camera towards this gameobject, scaled by the players position
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraTowardsItem : MonoBehaviour
{
    public float MaxPlayerDistance = 10;

    private float t = 0;

    private float defaultCameraZoom;

    private void Start()
    {
        defaultCameraZoom = Camera.main.depth;
    }
    private void Update()
    {
        t += Time.deltaTime;

        float distance = Vector2.Distance(PlayerBehaviour.Instance.transform.position, transform.position);

        Camera.main.depth = Mathf.Lerp(1, defaultCameraZoom, distance / MaxPlayerDistance);
    }
}
