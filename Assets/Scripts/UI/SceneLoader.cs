using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject creditsPage;

    public void GameSceneLoad()
    {
        SceneManager.LoadScene(4);
    }

    public void GameEndScreenLoad()
    {
        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        creditsPage.SetActive(true);
    }

    public void Close()
    {
        creditsPage.SetActive(false);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }


}
