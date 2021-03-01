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
        ShowLegend();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerId == -1)
        {
            SetLegend(!legend.enabled);
        }
    }

    public void ShowLegend()
    {
        SetLegend(true);
    }

    private void SetLegend(bool b)
    {
        legend.enabled = b;
        if (!legend.enabled)
        {
            GameManager.Instance.SetGameplayPause(false);
        }
        else
        {
            GameManager.Instance.SetGameplayPause(true);
        }
    }
}
