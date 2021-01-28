using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFireButton : MonoBehaviour
{
    public void OnClickFire()
    {
        InputManager.Instance.Inputs.Add("space");
    }
}
