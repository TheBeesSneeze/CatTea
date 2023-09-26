/*******************************************************************************
* File Name :         Traps.cs
* Author(s) :         Alex Bell, Toby Schamberger
* Creation Date :     9/25/2023
*
* Brief Description : Activates some spikes (or whatever) when the player 
* collides with the object this script is on.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Traps : MonoBehaviour
{
    [Tooltip("Object that is set active when character collides with this")]
    public GameObject ActivationObject;
    public float SecondsUntilActivation;
    public float SecondsUntilDeactivation;

    // Start is called before the first frame update
    void Start()
    {
        ActivationObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag.Equals("Player") || tag.Equals("Enemy") || tag.Equals("Boss"))
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(SecondsUntilActivation);
        ActivationObject.SetActive(true);

        yield return new WaitForSeconds(SecondsUntilDeactivation);
        ActivationObject.SetActive(false);
    }
}
