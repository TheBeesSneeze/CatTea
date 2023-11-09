/*******************************************************************************
* File Name :         SaveDataManager.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/4/2023
*
* Brief Description : Not sure about the scope of this yet.
* For now it is going to save settings and run #.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance;

    public SettingsDataClass SettingsData;
    public SaveDataClass SaveData;
    private string settingsPath;
    private string saveDataPath;

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
    private void Start()
    {
        //SettingsDataClass = Settings.Instance;
        settingsPath = string.Concat(Application.persistentDataPath, "/SettingsData.json");
        saveDataPath = string.Concat(Application.persistentDataPath, "/SaveData.json");

        LoadSettings();
    }

    /// <summary>
    /// saves the current settings to JSON
    /// </summary>
    public void SaveSettings()
    {
        Debug.Log(settingsPath);
        var stringifiedData = JsonUtility.ToJson(SettingsData);
        if (File.Exists(settingsPath))
        {
            File.WriteAllText(settingsPath, stringifiedData);
        }
        else
        {
            File.Create(settingsPath);
            File.WriteAllText(settingsPath, stringifiedData);
        }
    }

    /// <summary>
    /// Loads old settings from JSON. called when the thing is loaded
    /// </summary>
    public SettingsDataClass LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            string readText = File.ReadAllText(settingsPath);
            SettingsData = JsonUtility.FromJson<SettingsDataClass>(readText);

            Debug.Log("Sound: " + SettingsData.SoundVolume);

            return SettingsData;
        }

        Debug.LogWarning("Could not load settings. File does not exist");
        return null;
    }

    
}
