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
using UnityEngine;

public class UniversalVariables : MonoBehaviour
{
    public static UniversalVariables Instance { get; private set; }

    public GameObject EnemySpawningShadowPrefab;
    public GameObject UpgradeCollectionPrefab;
    public GameObject DefaultPlayerBulletPrefab;
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

}
