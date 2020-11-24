using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILifeBars : MonoBehaviour
{
    public GameObject[] PrefabsLifeBars;

    private List<List<Slider>> LifeBars = new List<List<Slider>>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<GameManager.Instance.NumberTeamsPlaying; i++)
        {
            LifeBars.Add(new List<Slider>());
            for (int j=0; j<GameManager.Instance.NumberAstronautsPerTeam; j++)
            {
                char team = '\0';
                switch (i)
                {
                    case 0:
                        team = 'A';
                        break;
                    case 1:
                        team = 'B';
                        break;
                    case 2:
                        team = 'C';
                        break;
                    case 3:
                        team = 'D';
                        break;
                }
                char astronaut = (j + 1).ToString()[0];
                string barName = "LifeBar" + team + astronaut;
                //Debug.Log(barName);
                //GameObject.Find() - look everywhere; Transform.Find() - only look in DIRECTZ Childs (1 Step below)
                //Slider newSlider = GameObject.Find(barName).GetComponent<Slider>();
                //LifeBars[i].Add(newSlider);

                GameObject newObj = Instantiate(PrefabsLifeBars[i], Vector2.zero, Quaternion.identity);

                LifeBars[i].Add(newObj.GetComponent<Slider>());
                LifeBars[i][j].transform.name = barName;
                LifeBars[i][j].transform.SetParent(GameObject.Find("LifeBarsTeam" + team).transform);
            }
        }
    }

    public void SetHealthValueOf(int teamNo, int astronautNo, int health)
    {
        LifeBars[teamNo - 1][astronautNo - 1].value = health;
    }

    public void DeactivateHealthBar(int teamNo, int astronautNo)
    {
        LifeBars[teamNo - 1][astronautNo - 1].gameObject.SetActive(false);
    }
}
