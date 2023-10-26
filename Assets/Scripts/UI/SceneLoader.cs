using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject creditsPage;
    public GameObject controlsPage;
    public GameObject pausePage;
    public void GameSceneLoad()
    {
        SceneManager.LoadScene(1);
    }

    public void GameEndScreenLoad()
    {
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

    public void Close()
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

    public void OpenPause()
    {
        Time.timeScale = 0;
        pausePage.SetActive(true);
    }

    public void ClosePause()
    {
        pausePage.SetActive(false);
        Time.timeScale = 1;
    }

}
