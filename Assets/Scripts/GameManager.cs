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
