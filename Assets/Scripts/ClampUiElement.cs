using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampUiElement : MonoBehaviour
{
    public GameObject uiElement;

    private Astronaut parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<Astronaut>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.isActive)
        {
            Vector3 thisPos = Camera.main.WorldToScreenPoint(this.transform.position);
            uiElement.transform.position = thisPos;
        }
    }
}
