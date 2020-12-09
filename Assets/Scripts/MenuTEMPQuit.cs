using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTEMPQuit : MonoBehaviour
{
    public void OnClickQuit()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
