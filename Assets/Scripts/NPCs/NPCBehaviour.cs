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
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCBehaviour : MonoBehaviour
{
    [Header("Dialogue")]
    //[SerializeField] private NPCScript DefaultDialogue;
    [Tooltip("Order of scripts this npc uses. DOES NOT NEED TO BE NINE! The list will loop after a while")]
    public NPCScript[] DialogueScripts;

    public NPCScript CurrentScript { 
        get { return CurrentScript; } 
        set { LoadScript(value);  } 
    }

    public enum Talking { Player, NPC, Nobody };

    private string Character1Name;
    private string Character2Name;

    private AudioClip Character1DialogueSound;
    private AudioClip Character2DialogueSound;

    private List<string> TextList = new List<string>();
    private List<Talking> WhoIsTalking = new List<Talking>();

    private List<Sprite> PlayerTalkingSprites = new List<Sprite>();
    private List<Sprite> NPCTalkingSprites = new List<Sprite>();

    [Header("Settings")]
    public bool LoopText;
    public static float ScrollSpeed = 0.05f;
    private float bobAnimationTime = 0.5f;

    [Header("Unity Stuff")]
    public GameObject ButtonPrompt;
    public TextMeshProUGUI TextBox;
    public GameObject DialogueCanvas;
    public Image PlayerSprite;
    public Image NPCSprite;

    private PlayerController player;
    private bool SkipText;
    public AudioSource dialogueSoundSource;

    private int textIndex = 0;
    private bool typing;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBehaviour>().GetComponent<PlayerController>();

        if(DialogueCanvas == null)
            DialogueCanvas = GameObject.Find("NPC Dialogue");

        if(dialogueSoundSource == null)
            dialogueSoundSource = DialogueCanvas.GetComponent<AudioSource>();

        int dialogueIndex = SaveDataManager.Instance.SaveData.RunNumber % DialogueScripts.Length;
        LoadScript(DialogueScripts[dialogueIndex]);

        ButtonPrompt.SetActive(false);
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

            player = collision.GetComponent<PlayerController>();
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

            player.Select.started -= ActivateSpeech;
        }
    }

    public void CancelSpeech()
    {
        player.IgnoreAllInputs = false;
        player.Pause.started -= Exit_text;
        player.SkipText.started -= Skip_text;

        textIndex = 0;
        if (ButtonPrompt != null)
            ButtonPrompt.SetActive(true);

        if(DialogueCanvas != null)
            DialogueCanvas.SetActive(false);
    }

    /// <summary>
    /// starts the coroutine
    /// </summary>
    /// <param name="obj"></param>
    public void ActivateSpeech(InputAction.CallbackContext obj)
    {
        player.IgnoreAllInputs = true;

        player.Pause.started += Exit_text;
        player.SkipText.started += Skip_text;

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

    public void Exit_text(InputAction.CallbackContext obj) 
    {
        CancelSpeech();
    }

    public void Skip_text(InputAction.CallbackContext obj)
    {
        SkipText = false;

        if(typing)  
            SkipText = true;
    }

    /// <summary>
    /// coroutine that does the scrolly typewriter text
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartText()
    {
        yield return new WaitForSeconds(ScrollSpeed);
        typing = true;
        DialogueCanvas.SetActive(true);

        SkipText = false;

        ChangeDialogueSprites();

        BobAnimations();

        SwitchTalkingSound();

        //typewriter text
        for (int i = 0; i < TextList[textIndex].Length + 1; i++)
        {
            if(SkipText)
            {
                Debug.Log("Skipping Text");
                SkipText = false;
                TextBox.text = TextList[textIndex];
                break;
            }
            
            TextBox.text = TextList[textIndex].Substring(0, i);

            PlayTalkingSound();

            yield return new WaitForSeconds(ScrollSpeed);
        }

        typing = false;
        textIndex++;

        if (textIndex >= TextList.Count && LoopText)
        {
            textIndex = 0;
        }
    }

    private void PlayTalkingSound()
    {
        dialogueSoundSource.Stop();
        dialogueSoundSource.pitch = Random.value;
        dialogueSoundSource.Play();
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

    private void SwitchTalkingSound()
    {
        if(dialogueSoundSource == null)
        {
            Debug.LogWarning("No audio source on " + gameObject.name);
            return;
        }

        if (WhoIsTalking[textIndex].Equals(Talking.Player))
            dialogueSoundSource.clip = Character1DialogueSound;

        if (WhoIsTalking[textIndex].Equals(Talking.NPC))
            dialogueSoundSource.clip = Character2DialogueSound;

        if (WhoIsTalking[textIndex].Equals(Talking.Nobody))
            dialogueSoundSource.clip = null;
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

    public void LoadScript(NPCScript Script)
    {
        if (Script == null)
            return;

        TextList = Script.TextList;

        WhoIsTalking = Script.WhoIsTalking;

        Character1Name = Script.Character1Name;
        Character2Name = Script.Character2Name;

        Character1DialogueSound = Script.Character1DialogueSound;
        Character2DialogueSound = Script.Character1DialogueSound; ;

        PlayerTalkingSprites = Script.PlayerTalkingSprites;
        NPCTalkingSprites = Script.NPCTalkingSprites;
    }
}
