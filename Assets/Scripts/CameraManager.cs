/*******************************************************************************
* File Name :         CameraManager.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/15/2023
*
* Brief Description : Accessed by rooms. Can follow the player. 
* Can lerp/slide between two points.
* 
* TODO:
* Follow player
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private bool followingPlayer;

    public Vector2 CameraCenterPos;

    //boring settings

    /// <summary>
    /// Moves camera to NewPosition. plays an animation
    /// </summary>
    public void MoveCamera(Transform NewPosition)
    {
        MoveCamera(NewPosition.position);
    }

    /// <summary>
    /// Moves camera to NewPosition. plays an animation
    /// </summary>
    public void MoveCamera(Vector3 NewPosition)
    {
        followingPlayer = false;

        NewPosition.z = -10;

        gameObject.transform.position = NewPosition;
    }
}
