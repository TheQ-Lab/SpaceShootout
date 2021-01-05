using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMLevelSelect : MonoBehaviour
{
    public string SelectedLevel = "";

    public void OnClickLevel(string str)
    {
        Debug.Log("Selected: " + str);
        SelectedLevel = str;

        transform.Find("StartButton").GetComponent<Button>().interactable = true;
    }

    public void OnClickStart()
    {
        if (SelectedLevel == "")
            return;
        Debug.Log("START GAME!");
        SceneManager.LoadScene(SelectedLevel);
    }
}
