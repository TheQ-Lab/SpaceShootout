using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMStart : MonoBehaviour
{
    public void OnClickQuit()
    {
        Debug.Log("QUIT GAME!");
        if (AppManager.Instance.IsWebGLBuild)
            Application.OpenURL(AppManager.Instance.ExitURL);
        else
            Application.Quit();
    }
}
