using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMLevelSelect : MonoBehaviour
{
    public GameObject[] levelPics;

    private void OnEnable()
    {
        //Coroutine that automatically selects button after set time if a level was already previously selected | fixed values
        if (AppManager.Instance.SelectedLevel == "Level01")
            StartCoroutine(ButtonHighlighter(levelPics[0].GetComponent<Button>()));
        else if (AppManager.Instance.SelectedLevel == "Level02")
            StartCoroutine(ButtonHighlighter(levelPics[1].GetComponent<Button>()));
        else if (AppManager.Instance.SelectedLevel == "Level03")
            StartCoroutine(ButtonHighlighter(levelPics[2].GetComponent<Button>()));
    }

    IEnumerator ButtonHighlighter(Button button)
    {
        yield return new WaitForSeconds(0.3f);
        button.Select();
    }

    public void OnClickLevel(string str)
    {
        Debug.Log("Selected: " + str);
        AppManager.Instance.SelectedLevel = str;

        transform.Find("StartButton").GetComponent<Button>().interactable = true;
    }

    public void OnClickStart()
    {
        if (AppManager.Instance.SelectedLevel == "")
        {
            Debug.Log("Select Level first!");
            return;
        }
        else if (AppManager.Instance.SelectedPlayerCount < 2)
        {
            Debug.Log("Invalid player count!");
            return;
        }
        Debug.Log("START GAME!");
        SceneManager.LoadScene(AppManager.Instance.SelectedLevel);
    }
}
