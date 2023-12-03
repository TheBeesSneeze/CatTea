/*******************************************************************************
* File Name :         UniversalVariables.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/2/2023
*
* Brief Description : Like a code warehouse. Also a singleton.
* Stores random variables that only really need to be stored once.
* Random prefabs and such. Less important values than those stored in GameManager
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniversalVariables : MonoBehaviour
{
    public static UniversalVariables Instance { get; private set; }

    public GameObject EnemySpawningShadowPrefab;
    public GameObject UpgradeCollectionPrefab;
    public GameObject DefaultPlayerBulletPrefab;

    [Header("Dialogue Canvas Stuff")]
    public TextMeshProUGUI TextBox;
    public GameObject DialogueCanvas;
    public GameObject DialogueArea;
    public Image PlayerSprite;
    public Image NPCSprite;

    public TextMeshProUGUI Name1;
    public TextMeshProUGUI Name2;

    public AudioSource dialogueSoundSource;

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

    // Start is called before the first frame update
    
    public SpriteRenderer GetCharacterSpriteRenderer(GameObject character)
    {
        SpriteRenderer sprite = character.GetComponent<SpriteRenderer>();

        if(sprite != null)
            return sprite;

        CharacterBehaviour characterBehaviour = character.GetComponent<CharacterBehaviour>();

        return characterBehaviour.CharacterSprite.GetComponent<SpriteRenderer>();
    }

    public SpriteRenderer GetBackgroundSpriteRenderer()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Background");
        if (background == null)
        {
            Debug.LogWarning("Cant find background in scene");
            return null;
        }
        return background.GetComponent<SpriteRenderer>();
    }

}
