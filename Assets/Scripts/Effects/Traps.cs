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

    private Coroutine spikesUpCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        ActivationObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ActivationObject == null)
        {
            Debug.LogWarning("Trap: " + gameObject.name + " does not have its activation object initialized in editor");
            return;
        }

        string tag = collision.tag;
        if (tag.Equals("Player") || tag.Equals("Enemy") || tag.Equals("Boss"))
        {
            if (spikesUpCoroutine != null)
                StopCoroutine(spikesUpCoroutine);

            spikesUpCoroutine = StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(SecondsUntilActivation);
        ActivationObject.SetActive(true);

        yield return new WaitForSeconds(SecondsUntilDeactivation);
        ActivationObject.SetActive(false);

        spikesUpCoroutine = null;
    }
}
