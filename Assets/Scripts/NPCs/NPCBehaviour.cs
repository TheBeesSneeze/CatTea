/*******************************************************************************
* File Name :         NPCBehavious.cs
* Author(s) :         Toby Schamberger, Sky Beal, Jay Embry
* Creation Date :     9/11/2023
*
* Brief Description : Basic NPC code that lets the player talk to npcs.
* (code stolen from Gorp Game)
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCBehaviour : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private NPCScript DefaultDialogue;

    public NPCScript CurrentScript { 
        get { return CurrentScript; } 
        set { LoadScript(value);  } 
    }

    public enum Talking { Player, NPC, Nobody };

    private List<string> TextList = new List<string>();
    private List<Talking> WhoIsTalking = new List<Talking>();

    [Tooltip("Leave a sprite null to make it not change")]
    private List<Sprite> PlayerTalkingSprites = new List<Sprite>();
    [Tooltip("Leave a sprite null to make it not change")]
    private List<Sprite> NPCTalkingSprites = new List<Sprite>();

    [Header("Settings")]
    public bool LoopText;
    [SerializeField] public float ScrollSpeed;
    [SerializeField] public TextMeshProUGUI TextBox;
    private int textIndex = 0;
    private float bobAnimationTime=0.5f;

    [Header("Unity Stuff")]
    public GameObject ButtonPrompt;
    private bool typing;
    public GameObject DialogueCanvas;
    public Image PlayerSprite;
    public Image NPCSprite;

    private void Start()
    {
        LoadScript(DefaultDialogue);
    }

    /// <summary>
    /// when on speaking box, can start text
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Player"))
        {
            if(ButtonPrompt != null)
                ButtonPrompt.SetActive(true);

            DefaultPlayerController player = collision.GetComponent<DefaultPlayerController>();
            player.Select.started += ActivateSpeech;
        }
    }

    /// <summary>
    /// when off speaking box, no start text
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            CancelSpeech();

            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(false);

            DefaultPlayerController player = collision.GetComponent<DefaultPlayerController>();
            player.Select.started -= ActivateSpeech;
        }
    }

    public void CancelSpeech()
    {
        textIndex = 0;
        if (ButtonPrompt != null)
            ButtonPrompt.SetActive(true);

        DialogueCanvas.SetActive(false);
    }

    /// <summary>
    /// starts the coroutine
    /// </summary>
    /// <param name="obj"></param>
    public void ActivateSpeech(InputAction.CallbackContext obj)
    {
        //if end dialogue
        if (!LoopText && textIndex == TextList.Count)
        {
            CancelSpeech();
            return;
        }

        if (!typing)
        {
            StartCoroutine(StartText());
        }

        if(ButtonPrompt!=null)
            ButtonPrompt.SetActive(false);
    }

    /// <summary>
    /// coroutine that does the scrolly text
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartText()
    {
        typing = true;
        DialogueCanvas.SetActive(true);

        ChangeDialogueSprites();

        BobAnimations();

        //typewriter text
        for (int i = 0; i < TextList[textIndex].Length + 1; i++)
        {
            TextBox.text = TextList[textIndex].Substring(0, i);
            yield return new WaitForSeconds(ScrollSpeed);
        }

        typing = false;
        textIndex++;

        if (textIndex >= TextList.Count && LoopText)
        {
            textIndex = 0;
        }
    }

    private void ChangeDialogueSprites()
    {
        if (textIndex < PlayerTalkingSprites.Count && PlayerTalkingSprites[textIndex] != null)
            PlayerSprite.sprite = PlayerTalkingSprites[textIndex];

        if (textIndex < NPCTalkingSprites.Count && NPCTalkingSprites[textIndex] != null)
            NPCSprite.sprite = NPCTalkingSprites[textIndex];
    }

    private void BobAnimations()
    {
        if (textIndex < WhoIsTalking.Count)
        {
            if (WhoIsTalking[textIndex] == Talking.Player)
                StartCoroutine(AnimateSprite(PlayerSprite.rectTransform));

            if (WhoIsTalking[textIndex] == Talking.NPC)
                StartCoroutine(AnimateSprite(NPCSprite.rectTransform));
        }
    }

    /// <summary>
    /// Animates the sprite by bobbing it up and down.
    /// </summary>
    /// <param name="Sprite"></param>
    /// <returns></returns>
    private IEnumerator AnimateSprite(RectTransform Sprite)
    {
        Vector3 startPosition = Sprite.position;
        Vector3 targetPosition = startPosition + new Vector3(0,50,0);

        float i = 0;

        while(i<0.5f)
        {
            i += 0.02f;

            Sprite.position = Vector3.Lerp(startPosition, targetPosition, i);

            yield return new WaitForSeconds(bobAnimationTime / 100);
        }

        while (i <= 1f)
        {
            i += 0.02f;

            Sprite.position = Vector3.Lerp(targetPosition, startPosition, i);

            yield return new WaitForSeconds(bobAnimationTime / 100);
        }
    }


    public void SetScript(NPCScript Script)
    {

    }
    public void LoadScript(NPCScript Script)
    {
        TextList = Script.TextList;

        WhoIsTalking = Script.WhoIsTalking;
        
        PlayerTalkingSprites = Script.PlayerTalkingSprites;
        NPCTalkingSprites = Script.NPCTalkingSprites;
    }
}
