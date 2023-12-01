/*******************************************************************************
* File Name :         SceneLoader.cs
* Author(s) :         Elda Osmani, Toby Schamberger
* Creation Date :     
*
* Brief Description : Code for the main menu, i think
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject creditsPage;
    public GameObject controlsPage;
    public GameObject pauseMenu;
    public Button loadGameButton;

    private void Start()
    {
        if(SaveDataManager.Instance == null)
        {
            return;
        }

        if(SaveDataManager.Instance.SaveData.TutorialCompleted)
        {
            if(loadGameButton == null)
            {
                return;
            }

            loadGameButton.interactable = true;
        }
    }

    public void NewGame()
    {
        SaveDataManager.Instance.ResetAllData();
        SceneManager.LoadScene(4);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
 //       SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);
    }

    public void GameEndScreenLoad()
    {
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(3);
    }

    public void Credits()
    {
        creditsPage.SetActive(true);
    }

    public void Controls()
    {
        controlsPage.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPage.SetActive(false);
    }

    public void CloseControls()
    {
        controlsPage.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void CutSceneLoad()
    {
        SceneManager.LoadScene(4);
    }

}
