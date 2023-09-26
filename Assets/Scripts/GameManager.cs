/*******************************************************************************
* File Name :         GameManager.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/11/2023
*
* Brief Description : Singleton which uh...
* Stores the run number
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

    [Header("Player Defined Settings")]
    public bool Rumble;

    public int DefaultChallengePoints;
    public int CurrentChallengePoints;

    [Header("Unity Stuff")]
    public GameObject MeleePlayerPrefab;
    public GameObject RangedPlayerPrefab;

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
    }

    /// <summary>
    /// destroys the current player and brings in the new one
    /// </summary>
    public void SwapPlayerAttackType(PlayerBehaviour currentPlayerBehaviour)
    {
        GameObject newPlayer = null;
        PlayerBehaviour newPlayerBehaviour = null;

        bool Default = currentPlayerBehaviour.PlayerWeapon == PlayerBehaviour.WeaponType.Default;
        bool melee = currentPlayerBehaviour.PlayerWeapon == PlayerBehaviour.WeaponType.Default;
        bool ranged = currentPlayerBehaviour.PlayerWeapon == PlayerBehaviour.WeaponType.Default;

        //Create new Ranged Player
        if (Default || melee)
        {
            newPlayer = Instantiate(RangedPlayerPrefab, currentPlayerBehaviour.transform.position, Quaternion.identity);
        }
        // new melee player
        else if(ranged)
        {
            newPlayer = Instantiate(MeleePlayerPrefab, currentPlayerBehaviour.transform.position, Quaternion.identity);
        }

        //replace all important values
        newPlayerBehaviour = newPlayer.GetComponent<PlayerBehaviour>();

        
    }
}
