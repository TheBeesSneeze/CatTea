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

        PlayerController.Instance.IgnoreAllInputs = true;
        Rigidbody2D body = PlayerBehaviour.Instance.GetComponent<Rigidbody2D>();
        body.velocity = Vector3.zero;

        PlayerController.Instance.Select.started += ActivateSpeech;
    }

    public override void CancelSpeech()
    {
        PlayerController.Instance.IgnoreAllInputs = false;
        PlayerController.Instance.MoveDirection = Vector3.zero;

        base.CancelSpeech();

        Room.OnBossTextEnded();

        Destroy(this);
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
