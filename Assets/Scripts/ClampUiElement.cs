using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampUiElement : MonoBehaviour
{
    [Tooltip("Should Autofill; Is Object to track")]
    public GameObject uiElement;
    public UITypeEnum UIType;

    public enum UITypeEnum {IsActive_Dependant, LifeBar, ActiveIndicator, ShotText, ShotBar};

    private Astronaut parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<Astronaut>();

        if (UIType == UITypeEnum.LifeBar)
        {
            char team = '\0';
            switch (parent.TeamNo)
            {
                case 1:
                    team = 'A';
                    break;
                case 2:
                    team = 'B';
                    break;
                case 3:
                    team = 'C';
                    break;
                case 4:
                    team = 'D';
                    break;
            }
            char astronaut = parent.AstronautNo.ToString()[0];
            uiElement = GameObject.Find("LifeBar" + team + astronaut);
        }
        else if (UIType == UITypeEnum.ActiveIndicator)
        {
            uiElement = CoolFunctions.FindInArray("ActiveIndicatorContainer", GameObject.FindGameObjectsWithTag("UIReferences"));
        }
        else if (UIType == UITypeEnum.ShotText)
        {
            uiElement = CoolFunctions.FindInArray("ShotTextContainer", GameObject.FindGameObjectsWithTag("UIReferences"));
        }
        else if (UIType == UITypeEnum.ShotBar)
        {
            uiElement = CoolFunctions.FindInArray("ShotBarContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UIType == UITypeEnum.IsActive_Dependant || UIType == UITypeEnum.ShotText || UIType == UITypeEnum.ShotBar)
        {
            if (!parent.isActive) { return; }
        }
        else if(UIType == UITypeEnum.LifeBar)
        {
            if(uiElement == null) {
                Start();
                return; 
            }
        }
        else if(UIType == UITypeEnum.ActiveIndicator)
        {
            if (!parent.isActive) { return; }
            Quaternion thisRot = this.transform.rotation;
            uiElement.transform.rotation = thisRot;
        }

        Vector3 thisPos = Camera.main.WorldToScreenPoint(this.transform.position);
        uiElement.transform.position = thisPos;
    }
}
