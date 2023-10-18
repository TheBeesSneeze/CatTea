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
* - On player shoot
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
    public Action<CharacterBehaviour> EnemyDamageAction;
    public Action PlayerDamageAction;
    public Action RoomEnterAction;
    public Action<AttackType> PlayerShootAction;//for each bullet
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
        EnemyDeathAction?.Invoke(eventPosition);
    }

    public void OnEnemyDamage(CharacterBehaviour eventCharacter)
    {
        EnemyDamageAction?.Invoke(eventCharacter);
    }

    public void OnPlayerDamage()
    {
        PlayerDamageAction?.Invoke();
    }

    public void OnRoomEnter()
    {
        RoomEnterAction?.Invoke();
    }

    /// <summary>
    /// Invokes every time a bullet is shot
    /// </summary>
    public void OnPlayerShoot(AttackType bullet)
    {
        PlayerShootAction?.Invoke(bullet);
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
