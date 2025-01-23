using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   public void gameStartGame()
    {
        SceneManager.LoadScene("Test Scene") ;
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
