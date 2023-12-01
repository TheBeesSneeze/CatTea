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

    private string character1Name;
    private string character2Name;

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
    [HideInInspector]public TextMeshProUGUI TextBox;
    [HideInInspector] public GameObject DialogueCanvas;
    [HideInInspector] public GameObject DialogueArea;
    [HideInInspector] public Image PlayerSprite;
    [HideInInspector] public Image NPCSprite;

    [HideInInspector] public TextMeshProUGUI Name1;
    [HideInInspector] public TextMeshProUGUI Name2;

    private bool SkipText;
    public AudioSource dialogueSoundSource;

    private int textIndex = 0;
    private bool typing;

    public virtual void Start()
    {
        PopulateCanvasVariables();

        if(DialogueCanvas == null)
            DialogueCanvas = GameObject.Find("NPC Dialogue");

        if (dialogueSoundSource == null)
        {
            dialogueSoundSource = DialogueCanvas.GetComponent<AudioSource>();
        }

        int run = SaveDataManager.Instance.SaveData.RunNumber;
        int dialogueIndex = Mathf.Min(run, DialogueScripts.Length - 1);

        LoadScript(DialogueScripts[dialogueIndex]);

        if(ButtonPrompt != null)
            ButtonPrompt.SetActive(false);
    }


    /// <summary>
    /// when on speaking box, can start text
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Player"))
        {
            if(ButtonPrompt != null)
                ButtonPrompt.SetActive(true);

            PlayerController.Instance.Select.started += ActivateSpeech;
        }
    }

    /// <summary>
    /// when off speaking box, no start text
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Player"))
        {
            if (ButtonPrompt != null)
                ButtonPrompt.SetActive(false);

            PlayerController.Instance.Select.started -= ActivateSpeech;
        }
    }

    public virtual void CancelSpeech()
    {
        Debug.Log("Cancelling speech");

        PlayerController.Instance.IgnoreAllInputs = false;
        PlayerController.Instance.Pause.started -= Exit_text;
        PlayerController.Instance.SkipText.started -= Skip_text;

        textIndex = 0;
        if (ButtonPrompt != null)
            ButtonPrompt.SetActive(true);

        if(DialogueCanvas != null)
            DialogueCanvas.SetActive(false);

    }

    /// <summary>
    /// starts the coroutine
    /// </summary>
    public void ActivateSpeech(InputAction.CallbackContext obj)
    {
        ActivateSpeech();
    }

    public virtual void ActivateSpeech()
    {
        Debug.Log("Activating speech");

        PlayerController.Instance.IgnoreAllInputs = true;

        PlayerController.Instance.Pause.started += Exit_text;
        PlayerController.Instance.SkipText.started += Skip_text;

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

        if (ButtonPrompt != null)
            ButtonPrompt.SetActive(false);
    }

    public virtual void Exit_text(InputAction.CallbackContext obj) 
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
    public virtual IEnumerator StartText()
    {
        yield return new WaitForSeconds(ScrollSpeed);
        typing = true;
        DialogueCanvas.SetActive(true);

        SkipText = false;

        ChangeDialogueSprites();

        BobAnimations();

        SwitchTalkingSound();

        SwitchNames();

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
    /// switches out names and flips text area
    /// </summary>
    private void SwitchNames()
    {
        if(Name1  == null || Name2 == null || DialogueArea == null)
        {
            Debug.LogWarning("Name Text boxes or Dialogue Area not defined in " + gameObject.name);

            return;
        }

        if (WhoIsTalking[textIndex].Equals(Talking.Player))
        {
            Name1.gameObject.SetActive(true);
            Name2.gameObject.SetActive(false);

            Name1.text = character1Name;

            DialogueArea.transform.localScale = new Vector3(1, 1, 1);
        }

        if (WhoIsTalking[textIndex].Equals(Talking.NPC))
        {
            Name1.gameObject.SetActive(false);
            Name2.gameObject.SetActive(true);

            Name2.text = character2Name;

            DialogueArea.transform.localScale = new Vector3(-1, 1, 1);
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

    public void PopulateCanvasVariables()
    {
        TextBox = UniversalVariables.Instance.TextBox;
        DialogueCanvas = UniversalVariables.Instance.DialogueCanvas;
        DialogueArea = UniversalVariables.Instance.DialogueArea;
        PlayerSprite = UniversalVariables.Instance.PlayerSprite;
        NPCSprite = UniversalVariables.Instance.NPCSprite;

        Name1 = UniversalVariables.Instance.Name1;
        Name2 = UniversalVariables.Instance.Name2;

        if (dialogueSoundSource == null)
            dialogueSoundSource = UniversalVariables.Instance.dialogueSoundSource;
    }

    public void LoadScript(NPCScript Script)
    {
        if (Script == null)
            return;

        TextList = Script.TextList;

        WhoIsTalking = Script.WhoIsTalking;

        character1Name = Script.Character1Name;
        character2Name = Script.Character2Name;

        Character1DialogueSound = Script.Character1DialogueSound;
        Character2DialogueSound = Script.Character1DialogueSound; ;

        PlayerTalkingSprites = Script.PlayerTalkingSprites;
        NPCTalkingSprites = Script.NPCTalkingSprites;
    }
}
