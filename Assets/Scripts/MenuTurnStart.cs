﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTurnStart : MonoBehaviour
{
    public Image[] TurnScreenColours;

    public void SelectTurnStartScreen(int team)
    {
        for (int i=1; i<=TurnScreenColours.Length; i++)
        {
            TurnScreenColours[i - 1].enabled = (i == team);
        }
    }

    public void OnClickStartTurn()
    {
        Debug.Log("START TURN");
        GameManager.Instance.SetGameplayPause(false);
    }
}
