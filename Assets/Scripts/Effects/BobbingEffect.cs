/*******************************************************************************
* File Name :         BobbingEffect.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/6/2023
*
* Brief Description : causes the object to kinda hover up and down
*****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    [Tooltip("Units up the thing moves")]
    public float UnitsToMove;
    [Tooltip("Seconds until the object reaches the top of its animation thing")]
    public float SecondsToReachTop;

    private Vector3 defaultPoint;

    // Start is called before the first frame update
    void Start()
    {
        defaultPoint = transform.position;

        StartCoroutine(BobbingAnimation());
    }

    private IEnumerator BobbingAnimation()
    {
        float t = 0;
        while(true)
        {
            t += Time.deltaTime % Mathf.PI;

            float tScaled = Mathf.Sin(t);

            float newY = defaultPoint.y + (UnitsToMove * tScaled);

            transform.position = new Vector2(defaultPoint.x, newY);

            yield return null;
        }
    }


    /*
    private IEnumerator BobbingAnimation()
    {
        float t = 0;
        while(this != null)
        {
            t = 0;

            while(t< SecondsToReachTop)
            {
                t += Time.deltaTime;
                float tScaled = t / SecondsToReachTop;

                float newY = defaultPoint.y + (UnitsToMove*tScaled);

                transform.position = new Vector2(defaultPoint.x, newY);

                yield return null;
            }

            while (t >0)
            {
                t -= Time.deltaTime;
                float tScaled = t / SecondsToReachTop;

                float newY = defaultPoint.y + (UnitsToMove * tScaled);

                transform.position = new Vector2(defaultPoint.x, newY);

                yield return null;
            }
        }
    }
    */
}
