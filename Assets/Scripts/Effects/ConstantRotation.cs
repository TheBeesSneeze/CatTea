/*******************************************************************************
* File Name :         ConstantRotation.cs
* Author(s) :         Toby Schamberger
* Creation Date :     sometime in early october
*
* Brief Description : literally just constantly rotates
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float Speed = 4f;

    // Update is called once per frame
    void Update()
    {
        float newAngle = (transform.eulerAngles.z + (Speed * Time.deltaTime)) % 360;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newAngle);
    }

    
}
