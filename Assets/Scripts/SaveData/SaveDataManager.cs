/*******************************************************************************
* File Name :         SaveDataManager.cs
* Author(s) :         Zach Abbott, Toby Schamberger
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
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;

        settingsPath = string.Concat(Application.persistentDataPath, "/SettingsData.json");
        saveDataPath = string.Concat(Application.persistentDataPath, "/SaveData.json");
        LoadSettings();
        LoadSaveData();
    }

    /// <summary>
    /// saves the current settings to JSON
    /// </summary>
    public void SaveSaveData()
    {
        Debug.Log(saveDataPath);
        var stringifiedData = JsonUtility.ToJson(SaveData);

        if (File.Exists(saveDataPath))
        {
            File.WriteAllText(saveDataPath, stringifiedData);
        }
        else
        {
            File.Create(saveDataPath);
            stringifiedData = JsonUtility.ToJson(SaveData);
            File.WriteAllText(saveDataPath, stringifiedData);
        }
    }
    /// <summary>
    /// Loads old settings from JSON. called when the thing is loaded
    /// </summary>
    public void LoadSaveData()
    {

        if (File.Exists(saveDataPath))
        {
            string readText = File.ReadAllText(saveDataPath);
            SaveData = JsonUtility.FromJson<SaveDataClass>(readText);
        }
        else
        {
            SaveData = new SaveDataClass(1, false, false);
            SaveSaveData();
        }
        Debug.LogWarning("Could not load save data. File does not exist");

        if (SaveData.GunUnlocked)
            PlayerBehaviour.Instance.OnGunUnlocked();
        else
            PlayerBehaviour.Instance.OnGunLocked();
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
            stringifiedData = JsonUtility.ToJson(SettingsData);
            File.WriteAllText(settingsPath, stringifiedData);
        }
    }
    /// <summary>
    /// Loads old settings from JSON. called when the thing is loaded
    /// </summary>
    public void LoadSettings()
    {

        if (File.Exists(settingsPath))
        {
            string readText = File.ReadAllText(settingsPath);
            SettingsData = JsonUtility.FromJson<SettingsDataClass>(readText);
            Debug.Log("Sound: " + SettingsData.SoundVolume);
        }
        else
        {
            SettingsData = new SettingsDataClass(1, 1, true);
            SaveSettings();
        }
        Debug.LogWarning("Could not load settings. File does not exist");
    }

}
