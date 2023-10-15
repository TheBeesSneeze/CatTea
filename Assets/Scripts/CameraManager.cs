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
    //private bool followingPlayer;
    [Tooltip("Units / sec the camera moves")]
    public float CameraSpeed;

    public Vector2 CameraCenterPos;

    //boring settings
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerBehaviour>().gameObject;
    }

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
        StopFollowPlayer();

        NewPosition.z = -10;

        gameObject.transform.position = NewPosition;
    }

    /// <summary>
    /// called automatically
    /// </summary>
    public void StopFollowPlayer()
    {
        //followingPlayer = false;
        transform.SetParent(null);
    }

    public void StartFollowPlayer()
    {
        //followingPlayer=true;
        //StartCoroutine(FollowPlayer());
        transform.SetParent(player.transform);
        transform.localPosition = new Vector3(0,0, transform.localPosition.z);
    }

    /*
    private IEnumerator FollowPlayer()
    {
        float z = this.transform.position.z;

        while (followingPlayer)
        {
            Vector3 playerPos = player.transform.position;
            playerPos.z = -10;

            this.transform.position = Vector2.MoveTowards(transform.position, playerPos, CameraSpeed / followPlayerFrames);

            Debug.Log(transform.position);

            transform.position = new Vector3(transform.position.x, transform.position.y, -10);

            yield return new WaitForSeconds(1 / followPlayerFrames);
        }
    }
    */
}
