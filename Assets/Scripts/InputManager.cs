using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance = null;

    public char arrowKey = '\0'; // \0 === empty char
    public List<string> Inputs; // has keys that are pressed down and not yet used

    Vector3 mouseCoord;

    private void Awake()
    {
        /*
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if(Instance != this)
        {
            Destroy(this.gameObject);
        }*/
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!GameManager.Instance.IsGameplayActive)
            return;*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Inputs.Contains("space"))
                Inputs.Add("space");
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Inputs.Contains("space"))
                Inputs.Remove("space");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!Inputs.Contains("tab"))
                Inputs.Add("tab");
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (Inputs.Contains("tab"))
                Inputs.Remove("tab");
        }

        mouseCoord = Camera.main.ScreenToViewportPoint(Input.mousePosition);


        if (Input.GetMouseButtonDown(1) && mouseCoord.y > 0.15)
        {
            if (!Inputs.Contains("mouse1"))
                Inputs.Add("mouse1");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (Inputs.Contains("mouse1"))
                Inputs.Remove("mouse1");
        }

        if (Input.GetMouseButtonDown(0) && mouseCoord.y > 0.15)
        {
            if (!Inputs.Contains("mouse0"))
                Inputs.Add("mouse0");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Inputs.Contains("mouse0"))
                Inputs.Remove("mouse0");
        }

        

        if (Input.GetMouseButtonDown(0) && mouseCoord.y > 0.15)
        {
            //Debug.Log("mouse " + mouseCoord.y);
            if (!Inputs.Contains("arrow")) Inputs.Add("arrow");

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Inputs.Contains("arrow")) Inputs.Remove("arrow");
        }

        


    }


}
