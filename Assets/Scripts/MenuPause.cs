using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public void OnClickResume()
    {
        Debug.Log("RESUME GAME");
        GameManager.Instance.SetGameplayPause(false);
    }

    public void OnClickMainMenu()
    {
        Debug.Log("MAIN MENU");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickQuit()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
