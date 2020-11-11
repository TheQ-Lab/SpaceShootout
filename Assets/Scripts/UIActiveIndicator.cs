using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActiveIndicator : MonoBehaviour
{
    public Color32[] colors;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void SelectColor(int i)
    {
        image.color = colors[i-1];
    }
}
