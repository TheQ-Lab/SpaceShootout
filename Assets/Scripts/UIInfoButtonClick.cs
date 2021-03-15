using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInfoButtonClick : MonoBehaviour, IPointerClickHandler
{
    public MenuInfo infoMenu;

    // Start is called before the first frame update
    void Start()
    {
        //ShowLegend();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerId == -1)
        {
            ShowLegend();
        }
    }

    public void ShowLegend()
    {
        Debug.Log("PAUSE GAME");
        GameManager.Instance.SetGameplayPause(true);
        infoMenu.gameObject.SetActive(true);
    }
}
