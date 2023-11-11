/*******************************************************************************
* File Name :         Settings.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/??/2023
*
* Brief Description : singleton. stores all the players settings! wow!
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [Header("Unity stuff")]
    public GameObject PauseMenu;
    public Slider SoundSlider;
    public Slider MusicSlider;

    [Header("These arent real")]
    public float SoundVolume=1; // 0 < x < 1
    public float MusicVolume=1; // "   "   "

    public bool ControllerVibration=true;

    [HideInInspector] public bool Paused;

    /// <summary>
    /// If there is an instance, and it's not me, delete myself.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    /// <summary>
    /// reloads saved settings
    /// </summary>
    private void Start()
    {
        // StartCoroutine(DelayedStart());
        UpdateSettingsFromJSON();
    }
    public void OpenPauseMenu()
    {
        SaveDataManager.Instance.LoadSettings();
        UpdateSettingsUI();

        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        Paused = true;
    }
    public void ClosePauseMenu()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        Paused = false;
    }

    public void UpdateSettingsFromJSON()
    {
        SaveDataManager.Instance.LoadSettings();

        SoundVolume = SaveDataManager.Instance.SettingsData.SoundVolume;
        MusicVolume = SaveDataManager.Instance.SettingsData.MusicVolume;
        ControllerVibration = SaveDataManager.Instance.SettingsData.ControllerVibration;

        UpdateSettingsUI(); 
    }

    public void UpdateSettingsUI()
    {
        SoundSlider.value = SoundVolume;
        MusicSlider.value = MusicVolume;
    }

    public void OnSoundSliderChange()
    {
        SoundVolume = SoundSlider.value;

        SaveDataManager.Instance.SettingsData.SoundVolume = SoundVolume;

        SaveDataManager.Instance.SaveSettings();

        UpdateAllAudioSources();
    }

    public void OnMusicSliderChange()
    {
        MusicVolume = MusicSlider.value;

        SaveDataManager.Instance.SettingsData.MusicVolume = MusicVolume;

        SaveDataManager.Instance.SaveSettings();

        UpdateAllAudioSources();
    }

    /// <summary>
    /// Updates every audioSource loaded
    /// </summary>
    private void UpdateAllAudioSources()
    {
        //searching for AudioSources instead of VolumeAdjuster because of debug reasons
        AudioSource[] allAudioSources = GameObject.FindObjectsOfType<AudioSource>();

        foreach(AudioSource audioSource in allAudioSources) 
        {
            VolumeAdjuster volumeAdjuster = audioSource.GetComponent<VolumeAdjuster>();

            if(volumeAdjuster == null)
            {
                Debug.LogWarning("No VolumeAdjuster component on " + audioSource.gameObject.name);
                continue;
            }

            volumeAdjuster.UpdateVolumeBySettings();
        }
    }
}
