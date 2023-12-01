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
    //[Tooltip("Units / sec the camera moves")]
    //public float CameraSpeed;
    public float MaxDistanceFromPlayer;
    [Tooltip("Make this number higher for less durastic movements")]
    public float MouseToPlayerScale;

    private bool followingPlayer;

    //boring settings
    private Transform player;
    private Transform mousePosition;
    private Coroutine followPlayerCoroutine;

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerBehaviour>().transform;
        mousePosition = GameObject.FindObjectOfType<RangedPlayerController>().RangedIcon.transform;
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
        followingPlayer = false;
        transform.SetParent(null);
    }

    public void StartFollowPlayer()
    {
        followingPlayer=true;
        //transform.SetParent(player.transform);
        //transform.localPosition = new Vector3(0,0, transform.localPosition.z);

        if (followPlayerCoroutine == null)
            followPlayerCoroutine = StartCoroutine(MoveCameraToMouse());
    }

    private IEnumerator MoveCameraToMouse()
    {
        while(followingPlayer)
        {
            if(!PlayerController.Instance.IgnoreAllInputs)
            {
                Vector3 newPosition = GetNewCameraPosition();
                newPosition.z = -10;
                transform.position = newPosition;
            }

            //yield return null;
            yield return new WaitForFixedUpdate();
        }
        followPlayerCoroutine = null;
    }

    /// <summary>
    /// Gets new camera position relaitvie to the player
    /// </summary>
    /// <returns>does not fix z axis!</returns>
    private Vector3 GetNewCameraPosition()
    {
        Vector3 difference = mousePosition.position - player.position;
        difference = difference / MouseToPlayerScale;

        Vector3 newPosition = player.position + difference;

        //mouse position is within good range :)
        if (Vector2.Distance((Vector2)player.position, newPosition) < MaxDistanceFromPlayer)
        {
            return player.position + difference;
        }

        //rescale position to fix maxdistance
        newPosition = player.position + (difference.normalized * MaxDistanceFromPlayer);

        return newPosition;
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
