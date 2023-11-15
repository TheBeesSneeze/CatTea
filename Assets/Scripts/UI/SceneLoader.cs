using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject creditsPage;
    public GameObject controlsPage;
    public GameObject pauseMenu;

    public void GameSceneLoad()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.UnloadSceneAsync(1);
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

    public void CutSceneLoad()
    {
        SceneManager.LoadScene(4);
    }

}
