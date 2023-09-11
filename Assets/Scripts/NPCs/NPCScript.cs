/*******************************************************************************
* File Name :         NPCScript.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/11/2023
*
* Brief Description : Scriptable object that stores ONE dialogue interraction.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCScript", menuName = "NPC Script")]
public class NPCScript : ScriptableObject
{
    [SerializeField][TextArea] public List<string> TextList = new List<string>();

    public List<NPCBehaviour.Talking> WhoIsTalking = new List<NPCBehaviour.Talking>();

    [Tooltip("Leave a sprite null to make it not change")]
    public List<Sprite> PlayerTalkingSprites = new List<Sprite>();
    [Tooltip("Leave a sprite null to make it not change")]
    public List<Sprite> NPCTalkingSprites = new List<Sprite>();
}
