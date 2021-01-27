using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShotStats : MonoBehaviour
{
    private Text angleText, powerText;

    private int angle, power;

    private void Start()
    {
        angleText = transform.Find("AngleText").GetComponent<Text>();
        powerText = transform.Find("PowerText").GetComponent<Text>();
    }

    public void SetActive(bool active)
    {
        if (!active)
        {
            angleText.text = "-";
            powerText.text = "-";
        }
    }

    public void SetAngle(int newAngle)
    {
        angle = newAngle;
        UpdateDisplayedTexts();
    }

    public void SetPower(int newPower)
    {
        power = newPower;
        UpdateDisplayedTexts();
    }

    private void UpdateDisplayedTexts()
    {
        angleText.text = angle.ToString();
        powerText.text = power.ToString();
    }
}
