using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISoundButtonClick : MonoBehaviour, IPointerClickHandler
{

    public Sprite soundOn;
    public Sprite soundOff;

    Image myImage;  
    bool toggle;

    void Start()
    {
        myImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {

        if (pointerEventData.pointerId == -1)
        {
            toggle = !toggle;

            if (!toggle)
            {
                myImage.sprite = soundOn;
                AudioListener.volume = 1f;
            }

            else
            {
                myImage.sprite = soundOff;
                AudioListener.volume = 0f;
            }
        }
    }
}
