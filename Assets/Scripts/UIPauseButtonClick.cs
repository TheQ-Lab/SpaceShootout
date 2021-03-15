using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPauseButtonClick : MonoBehaviour, IPointerClickHandler
{

    public MenuPause pauseMenu;

    void Start()
    {
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {

        if (pointerEventData.pointerId == -1)
        {
            Debug.Log("PAUSE GAME");
            GameManager.Instance.SetGameplayPause(true);
            pauseMenu.gameObject.SetActive(true);
        }
    }
}
