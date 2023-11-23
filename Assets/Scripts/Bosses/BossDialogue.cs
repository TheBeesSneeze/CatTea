/*******************************************************************************
* File Name :         BossDialogue.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/22/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossDialogue : NPCBehaviour
{
    public BossRoom Room;

    public override void Start()
    {
        //do nothing on purpose
    }

    public void Initialize()
    {
        base.Start();
        PlayerController.Instance.Select.started += ActivateSpeech;
    }

    public override void CancelSpeech()
    {
        base.CancelSpeech();

        Room.OnBossTextEnded();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //do nothing on purpose
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        //do nothing on purpose
    }
}
