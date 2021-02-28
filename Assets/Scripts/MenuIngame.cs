using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIngame : MonoBehaviour
{
    public void OnClickPause()
    {
        Debug.Log("PAUSE GAME");
        GameManager.Instance.SetGameplayPause(true);
    }

    public void ShowLegend()
    {
        transform.Find("InfoButton").GetComponent<UIInfoButtonClick>().ShowLegend();
    }
}
