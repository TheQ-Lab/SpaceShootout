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

        Vector3 mouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && mouseCoord.y > -10)
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
