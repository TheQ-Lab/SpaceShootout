using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInfoButtonClick : MonoBehaviour, IPointerClickHandler
{
    private Image legend;

    // Start is called before the first frame update
    void Start()
    {
        legend = transform.Find("LegendScreen").GetComponent<Image>();
        legend.enabled = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerId == -1)
        {
            legend.enabled = !legend.enabled;
            if (!legend.enabled)
            {
                GameManager.Instance.SetGameplayPause(false);
            }
            
        }
    }

    public void ShowLegend()
    {
        legend.enabled = true;
    }
}
