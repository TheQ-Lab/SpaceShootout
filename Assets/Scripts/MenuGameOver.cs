using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    public Image[] WinScreenColours;

    public void SelectWinScreen(int team)
    {
        for (int i = 1; i <= WinScreenColours.Length; i++)
        {
            WinScreenColours[i - 1].enabled = (i == team);
        }

        transform.Find("MainMenuButton").GetComponent<Image>().enabled = true;
        transform.Find("ReplayButton").GetComponent<Image>().enabled = true;
    }

    public void OnClickMainMenu()
    {
        Debug.Log("MAIN MENU");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickReplay()
    {
        Debug.Log("RESTART LEVEL");
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.buildIndex);
    }
}
