/*******************************************************************************
* File Name :         GameManager.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/11/2023
*
* Brief Description : Singleton which uh...
* Stores the run number
* As upgrades are acquired through the run, GameManager needs to keep track of which
* ones the players gotten so far, so theres no duplicates.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [Tooltip("List of every non-permenant upgrade")]
    public List<GameObject> UpgradePool;
    [HideInInspector]public List<GameObject> CurrentUpgradePool; 

    public int DefaultChallengePoints;
    public int CurrentChallengePoints;

    public RoomType CurrentRoom;

    private SpriteRenderer backgroundSprite;

    protected AudioSource backgroundMusicPlayer;
    private float defaultBGMVolume;

    //magic numbers
    protected float secondsBetweenDestroyingAttacks = 0.1f; 

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

    private void Start()
    {
        backgroundSprite = UniversalVariables.Instance.GetBackgroundSpriteRenderer();
        backgroundMusicPlayer = GameObject.Find("Background Music").GetComponent<AudioSource>();
        defaultBGMVolume = backgroundMusicPlayer.volume;

        CurrentChallengePoints = DefaultChallengePoints;

        CurrentUpgradePool = new List<GameObject>(UpgradePool); //copys list awesome

        if(SaveDataManager.Instance.SaveData.TutorialCompleted)
        {
            EnterHub(); 
        }
        else
            EnterTutorial();
    }

    /// <summary>
    /// Fades current playing song into another
    /// Rooms do not use this code. i dont have time to do that.
    /// this is only used when you clear enemy/boss rooms
    /// </summary>
    public void TransitionMusic(AudioClip newSong)
    {
        StartCoroutine(TransitionMusicCoroutine(newSong));
    }

    private IEnumerator TransitionMusicCoroutine(AudioClip newSong)
    {
        if (backgroundMusicPlayer.volume > 0)
        {
            ScaleBackgroundMusic(0);
            yield return new WaitForSeconds(RoomTransition.Instance.TotalTransitionSeconds / 2);
        }

        backgroundMusicPlayer.clip = newSong;
        ScaleBackgroundMusic(1);
    }

    public void ScaleBackgroundMusic(float targetVolume)
    {
        StartCoroutine(ScaleBackgroundMusicCoroutine(targetVolume));
    }

    /// <summary>
    /// Turns up or down the music volume, over the same amount of seconds it takes to transition rooms
    /// </summary>
    /// <param name="TargetVolume"> 0-1</param>
    /// <returns></returns>
    private IEnumerator ScaleBackgroundMusicCoroutine(float TargetVolume)
    {
        float startingVolume = backgroundMusicPlayer.volume;
        float scaleSeconds = RoomTransition.Instance.TotalTransitionSeconds / 2;
        float time = 0;

        while (time < scaleSeconds)
        {
            time += Time.deltaTime;

            float t = time / scaleSeconds;
            float tScaled = Mathf.Pow(t, 1f / 2f);

            float volumeScale = SaveDataManager.Instance.SettingsData.MusicVolume * defaultBGMVolume;
            backgroundMusicPlayer.volume = volumeScale * Mathf.Lerp(startingVolume, TargetVolume, tScaled);

            yield return null;
        }
    }

    private void EnterTutorial()
    {
        TutorialRoom tutorial = GameObject.FindObjectOfType<TutorialRoom>();
        tutorial.EnterRoom();
        Camera.main.backgroundColor = tutorial.BackgroundColor;
        backgroundSprite.color = tutorial.BackgroundColor;
    }
    /// <summary>
    /// loads in the hub on start
    /// </summary>
    private void EnterHub()
    {
        HubRoom hub = GameObject.FindObjectOfType<HubRoom>();
        hub.EnterRoom();
        Camera.main.backgroundColor = hub.BackgroundColor;
        backgroundSprite.color = hub.BackgroundColor;

    }

    /// <summary>
    /// intended to be called by enemies/bosses when they die to despawn all attacks.
    /// </summary>
    /// <param name="destroyObjects"></param>
    public void DestroyAllObjectsInList(List<GameObject> destroyObjects)
    {
        StartCoroutine(DestroyAllObjectsInListCoroutine(destroyObjects));
    }

    private IEnumerator DestroyAllObjectsInListCoroutine(List<GameObject> destroyObjects)
    {
        yield return new WaitForSeconds(secondsBetweenDestroyingAttacks);

        for (int i = 0; i < destroyObjects.Count; i++)
        {
            GameObject attack = destroyObjects[i];

            if (attack == null)
                continue;

            Destroy(attack);

            yield return new WaitForSeconds(secondsBetweenDestroyingAttacks);
        }
    }

    /*
    //toby toby toby i am pretending to be toby

    If(Player = Dead)
    {
        uhhhhh;
        ResetRun();
        TripleNPCDialogueBarrage;
        Debug.Log("you have died. you are but a feeble mortal in my palms. my little plaything. your life is but a meaningless drop in the ocean of souls. i hope you know your feeble mind can't even comprehend this madness.");
    }
    */
}
