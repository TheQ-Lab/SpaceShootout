using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance = null;

    public int SelectedPlayerCount = 0;
    public string SelectedLevel = "";
    public bool IsWebGLBuild = true;
    public string ExitURL = "www.marlin-ferlmann.de";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }
    }


}
