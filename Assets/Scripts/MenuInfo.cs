using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (InputManager.Instance.Inputs.Contains("space"))
        {

            InputManager.Instance.Inputs.Remove("space");
            OnClickLegend();
            gameObject.SetActive(false);
        }
    }

    public void OnClickLegend()
    {
        Debug.Log("RESUME GAME");
        GameManager.Instance.SetGameplayPause(false);
    }
}
