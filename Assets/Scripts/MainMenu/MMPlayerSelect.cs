using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMPlayerSelect : MonoBehaviour
{
    public void OnClickPlayerAmmountSelect(int num)
    {
        Debug.Log("Selected: " + num + " players");
        AppManager.Instance.SelectedPlayerCount = num;
    }
}
