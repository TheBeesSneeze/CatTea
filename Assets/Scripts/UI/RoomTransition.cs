/*******************************************************************************
* File Name :         RoomTransition.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/27/2023
*
* Brief Description : singleton. called when doors switch rooms
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public static RoomTransition Instance { get; private set; }
    public float TotalTransitionSeconds;

    public RectTransform TransitionSquare;

    //magic numbers
    private float targetSize = 1600;
    private float tScale = 0.90f;

    private GameObject player;
    private Coroutine transitonCoroutine;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    /// <summary>
    /// Creates a cool effect for switching rooms.
    /// Calls ExitRoom and EnterRoom appropriately.
    /// </summary>
    public void SwitchRooms(RoomType currentRoom, RoomType nextRoom)
    {
        if(transitonCoroutine != null)
            StopCoroutine(transitonCoroutine);

        transitonCoroutine = StartCoroutine(TransitionEffect(currentRoom, nextRoom));
    }
    
    /// <summary>
    /// also calls exitroom and startRoom
    /// </summary>
    /// <param name="currentRoom"></param>
    /// <param name="nextRoom"></param>
    /// <returns></returns>
    private IEnumerator TransitionEffect(RoomType currentRoom, RoomType nextRoom)
    {
        TransitionBackgroundColor(currentRoom, nextRoom);

        MoveBox(currentRoom.Door.transform.position);
        StartCoroutine(ScaleTransitionBox(0, targetSize, tScale));

        if (currentRoom != null)
        {
            currentRoom.ExitRoom();
        }

        yield return new WaitForSeconds(TotalTransitionSeconds/1.5f);

        if (nextRoom != null)
            nextRoom.EnterRoom();

        MoveBox(player.transform.position);
        StartCoroutine(ScaleTransitionBox(targetSize, 0, tScale));
    }

    /// <summary>
    /// Resizes the transition cover box over TotalTransitionSeconds / 2
    /// </summary>
    /// <param name="startScale"></param>
    /// <param name="endScale"></param>
    /// <returns></returns>
    private IEnumerator ScaleTransitionBox(float startScale, float endScale, float scaleFactor)
    {
        float transitionSliceSeconds = TotalTransitionSeconds / 2;

        float t = 0; // 0 <= t <= 1

        while (t < transitionSliceSeconds)
        {
            t += Time.deltaTime;
            float tScaled = Mathf.Pow(t, scaleFactor);

            float scale = Mathf.Lerp(startScale, endScale, tScaled);

            TransitionSquare.sizeDelta = new Vector2(scale, scale);

            yield return null;
        }
    }

    /// <summary>
    /// moves the transition box to the vector. does stuff to make it consistent with, ok nevermind
    /// i dont even know what it does. i am so tired.
    /// </summary>
    /// <param name="boxCenterPoint"></param>
    private void MoveBox(Vector3 boxCenterPoint)
    {
        TransitionSquare.position = boxCenterPoint;
        TransitionSquare.position = TransitionSquare.InverseTransformPoint(boxCenterPoint);
        TransitionSquare.localPosition = Vector2.zero;
    }

    /// <summary>
    /// starts the background transition coroutine, if deemed necessary.
    /// </summary>
    private void TransitionBackgroundColor(RoomType currentRoom, RoomType nextRoom)
    {
        if(nextRoom.BackgroundColor.a < 1)
        {
            Debug.Log(nextRoom.gameObject.name + " uses the same background color as the previous room");
            return;
        }

        StartCoroutine(TransitionBackgroundColorCoroutine(Camera.main.backgroundColor, nextRoom.BackgroundColor));
    }

    /// <summary>
    /// Waits until slightly before the screen cover starts shrinking to transition colors.
    /// takes slightly longer than the cover screen to shrink.
    /// </summary>
    private IEnumerator TransitionBackgroundColorCoroutine(Color startColor, Color targetColor)
    {
        float secondsToChangeBackgroundColor = TotalTransitionSeconds * 1.5f;

        float t = 0; // 0 <= t <= 1
        while(t < secondsToChangeBackgroundColor)
        {
            t += Time.deltaTime;
            //float tScaled = Mathf.Pow(t, 3);

            Camera.main.backgroundColor = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }
    }

    /// <summary>
    /// Debug command which instantly skips to the next room
    /// </summary>
    public void ForceRoomSkip()
    {
        RoomType currentRoom = GameManager.Instance.CurrentRoom;
        RoomType nextRoom = currentRoom.Door.OutputRoom;

        SwitchRooms(currentRoom, nextRoom);
    }
}
