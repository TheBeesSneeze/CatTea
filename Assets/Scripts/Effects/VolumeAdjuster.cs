
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************************************
* File Name :         VolumeAdjuster.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/5/2023
*
* Brief Description : Automatically scales volume according to the settings.
*****************************************************************************/

[RequireComponent(typeof(AudioSource))]
public class VolumeAdjuster : MonoBehaviour
{
    public enum SoundType
    {
        SoundEffect,
        BackgroundMusic
    }
    public SoundType Type;

    private float defaultVolume;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;

        UpdateVolumeBySettings();
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);

        
    }

    public void UpdateVolumeBySettings()
    {
        if(Type == SoundType.SoundEffect) 
            audioSource.volume = defaultVolume * Settings.Instance.SoundVolume;

        if (Type == SoundType.BackgroundMusic) 
            audioSource.volume = defaultVolume * Settings.Instance.MusicVolume;
    }
}
