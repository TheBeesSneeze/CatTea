/*******************************************************************************
* File Name :         GameEvents.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/4/2023
*
* "Brief" Description : Listens for certain events. Might be deleted and merged into
* upgrades tbh
* Listens for:
* - Enemy Death
* - Enemy takes damage
* - Player takes damage
* - On room enter
* - On player gun
* - On player sword
* - On player dash
* 
* Code for these functions should
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    public Action<Vector3> EnemyDeathAction;
    public Action EnemyDamageAction;
    public Action PlayerDamageAction;
    public Action RoomEnterAction;
    public Action PlayerGunAction;
    public Action PlayerSwordAction;
    public Action PlayerDashAction;

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

    //making these was so brain numbing

    public void OnEnemyDeath(Vector3 eventPosition)
    {
        Debug.Log("Enemy die");

        EnemyDeathAction?.Invoke(eventPosition);
    }

    public void OnEnemyDamage()
    {
        PlayerDamageAction?.Invoke();
    }

    public void OnPlayerDamage()
    {
        PlayerDamageAction?.Invoke();
    }

    public void OnRoomEnter()
    {
        RoomEnterAction?.Invoke();
    }

    public void OnPlayerGun()
    {
        PlayerGunAction?.Invoke();
    }

    public void OnPlayerSword()
    {
        PlayerSwordAction?.Invoke();
    }

    public void OnPlayerDash()
    {
        PlayerDashAction?.Invoke();
    }

}
