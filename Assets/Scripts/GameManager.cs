/*******************************************************************************
* File Name :         GameManager.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/11/2023
*
* Brief Description : Singleton which uh...
* Stores the run number
* As upgrades are acquired through the run, GameManager needs to keep track of which
* ones the players gotten so far, so theres no duplicates.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("0 - 8, for how many times the players died")]
    public uint RunNumber;

    [Tooltip("List of every non-permenant upgrade")]
    public List<GameObject> UpgradePool;
    [HideInInspector]public List<GameObject> CurrentUpgradePool; 

    [Header("Player Defined Settings")]
    public bool Rumble;

    public int DefaultChallengePoints;
    public int CurrentChallengePoints;

    public RoomType CurrentRoom;

    //magic numbers
    protected float secondsBetweenDestroyingAttacks = 0.1f; 

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
        CurrentChallengePoints = DefaultChallengePoints;

        CurrentUpgradePool = new List<GameObject>(UpgradePool); //copys list awesome

        EnterHub();
    }

    /// <summary>
    /// loads in the hub on start
    /// </summary>
    private void EnterHub()
    {
        HubRoom hub = GameObject.FindObjectOfType<HubRoom>();
        hub.EnterRoom();
    }

    /// <summary>
    /// intended to be called by enemies/bosses when they die to despawn all attacks.
    /// </summary>
    /// <param name="destroyObjects"></param>
    public void DestroyAllObjectsInList(List<GameObject> destroyObjects)
    {
        StartCoroutine(DestroyAllObjectsInListCoroutine(destroyObjects));
    }

    private IEnumerator DestroyAllObjectsInListCoroutine(List<GameObject> destroyObjects)
    {
        yield return new WaitForSeconds(secondsBetweenDestroyingAttacks);

        for (int i = 0; i < destroyObjects.Count; i++)
        {
            GameObject attack = destroyObjects[i];

            if (attack == null)
                continue;

            Destroy(attack);

            yield return new WaitForSeconds(secondsBetweenDestroyingAttacks);
        }
    }

    /*
    //toby toby toby i am pretending to be toby

    If(Player = Dead)
    {
        uhhhhh;
        ResetRun();
        TripleNPCDialogueBarrage;
        Debug.Log("you have died. you are but a feeble mortal in my palms. my little plaything. your life is but a meaningless drop in the ocean of souls. i hope you know your feeble mind can't even comprehend this madness.");
    }
    */
}
