using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampUiElement : MonoBehaviour
{
    public GameObject uiElement;
    public UITypeEnum UIType;

    public enum UITypeEnum {ShotBarArrowEtc, LifeBar};

    private Astronaut parent;

    // Start is called before the first frame update
    void Start()
    {
        if (UIType == UITypeEnum.ShotBarArrowEtc)
        {
            parent = GetComponentInParent<Astronaut>();
        }
        else if (UIType == UITypeEnum.LifeBar)
        {
            parent = GetComponentInParent<Astronaut>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (UIType == UITypeEnum.ShotBarArrowEtc)
        {
            if (!parent.isActive) { return; }
        }
        else if(UIType == UITypeEnum.LifeBar)
        {
            if(uiElement == null) { return; }
        }
        Vector3 thisPos = Camera.main.WorldToScreenPoint(this.transform.position);
        uiElement.transform.position = thisPos;
    }
}
