using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{


    public void GameSceneLoad()
    {
        SceneManager.LoadScene(4);
    }

    public void GameEndScreenLoad()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }


}
