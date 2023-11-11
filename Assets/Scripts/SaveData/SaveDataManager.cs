/*******************************************************************************
* File Name :         SaveDataManager.cs
* Author(s) :         Zach Abbott, Toby Schamberger
* Creation Date :     11/4/2023
*
* Brief Description : Not sure about the scope of this yet.
* For now it is going to save settings and run #.
* 
* guys i spent like an hour of bashing my head into a wall and have decided
* to use player prefs. i am sorry zach.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public enum DebugMode { NormalMode, ClearDataOnStart, CheatyMode }
    [Tooltip("CheatyMode: start with everything")]
    public DebugMode debugMode;

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

        /*
        settingsPath = string.Concat(Application.persistentDataPath, "/SettingsData.json");
        saveDataPath = string.Concat(Application.persistentDataPath, "/SaveData.json");

        Debug.Log(saveDataPath);
        VeryifyFilesExist();
        */

        LoadSettings();
        LoadSaveData();
    }
    private void Start()
    {
        if (debugMode == DebugMode.ClearDataOnStart)
        {
            ResetAllData();
        }

        if(debugMode == DebugMode.CheatyMode)
        {
            UnlockEverything();
        }
    }

    public void ResetAllData()
    {
        SaveData = new SaveDataClass(0, false, false);
        SettingsData = new SettingsDataClass(1, 1, true);

        SaveSaveData();
        SaveSettings();
    }

    private void UnlockEverything()
    {
        SaveData = new SaveDataClass(0, true, true);

        SaveSaveData();
    }

    /// <summary>
    /// saves the current settings to JSON
    /// </summary>
    public void SaveSaveData()
    {
        if(SaveData == null)
        {
            Debug.LogWarning("No save data");
            return;
        }

        PlayerPrefs.SetInt("Run Number", SaveData.RunNumber);
        PlayerPrefs.SetInt("Gun Unlocked", SaveData.GunUnlocked.ConvertTo<int>());
        PlayerPrefs.SetInt("Tutorial Completed", SaveData.TutorialCompleted.ConvertTo<int>());

        PlayerPrefs.Save();
        /*
        Debug.Log(saveDataPath);
        var stringifiedData = JsonUtility.ToJson(SaveData);

        if (File.Exists(saveDataPath))
        {
            File.WriteAllText(saveDataPath, stringifiedData);
        }
        else
        {
            File.Create(saveDataPath);

            if(SaveData == null)
                SaveData = new SaveDataClass(1, false, false);

            stringifiedData = JsonUtility.ToJson(SaveData);
            File.WriteAllText(saveDataPath, stringifiedData);
        }
        */
    }
    /// <summary>
    /// Loads old settings from JSON. called when the thing is loaded
    /// </summary>
    public void LoadSaveData()
    {
        int runNumber = PlayerPrefs.GetInt("Run Number", 0);
        bool tutorial = (PlayerPrefs.GetInt("Tutorial Completed", 0) == 1);
        bool gun = (PlayerPrefs.GetInt("Gun Unlocked", 0) == 1);
        
        SaveData = new SaveDataClass(runNumber, tutorial, gun);

        /*
        if (File.Exists(saveDataPath))
        {
            string readText = File.ReadAllText(saveDataPath);
            SaveData = JsonUtility.FromJson<SaveDataClass>(readText);
        }
        else
        {
            Debug.LogWarning("Could not load save data. File does not exist");
            SaveData = new SaveDataClass(1, false, false);
            SaveSaveData();
        }

        if (SaveData.GunUnlocked)
            PlayerBehaviour.Instance.OnGunUnlocked();
        else
            PlayerBehaviour.Instance.OnGunLocked();
        */
    }

    /// <summary>
    /// saves the current settings to JSON
    /// </summary>
    public void SaveSettings()
    {
        if (SettingsData == null)
        {
            Debug.LogWarning("No settings data");
            return;
        }

        PlayerPrefs.SetFloat("Sound Volume", SettingsData.SoundVolume);
        PlayerPrefs.SetFloat("Music Volume", SettingsData.MusicVolume);
        PlayerPrefs.SetInt("Controller Vibration", (SettingsData.ControllerVibration ? 1 : 0));

        PlayerPrefs.Save();
        /*
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
        */
    }
    /// <summary>
    /// Loads old settings from JSON. called when the thing is loaded
    /// </summary>
    public void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("Sound Volume", 1);
        float music = PlayerPrefs.GetFloat("Music Volume", 1);
        bool vibrate = (PlayerPrefs.GetInt("Controller Vibration", 1) == 1);

        SettingsData = new SettingsDataClass(volume, music, vibrate);
        /*
        if (File.Exists(settingsPath))
        {
            string readText = File.ReadAllText(settingsPath);
            SettingsData = JsonUtility.FromJson<SettingsDataClass>(readText);
            Debug.Log("Sound: " + SettingsData.SoundVolume);
        }
        else
        {
            Debug.LogWarning("Could not load settings. File does not exist");
            SettingsData = new SettingsDataClass(1, 1, true);
            SaveSettings();
        }
        */
    }
}
