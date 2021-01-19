using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // for all UI-Elements

public class UIShotBar : MonoBehaviour
{
    private Slider slider;
    
    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void SetAngle(int angle)
    {
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270 + angle));
    }

    public void SetPower(int power)
    {
        slider.value = 0.01f * power;
    }
}
