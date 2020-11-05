using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int PlayerAtTurn = 1;
    public List<GameObject> Astronauts;
    //public List<List<GameObject>> Astronauts;  // for later, when we have multiple Astronauts per Team use this 2-Dimensional List
    //public List<int> LastActiveAstronaut // which Astronaut per Team was last active

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandOverTurn()
    {
        int nextPlayerAtTurn = PlayerAtTurn + 1;
        if (PlayerAtTurn > Astronauts.Count-1)
        {
            nextPlayerAtTurn = 1;
        }
        Debug.Log("Turn Over -> " + nextPlayerAtTurn);


        Astronaut oldAstronautScript = Astronauts[PlayerAtTurn-1].GetComponent<Astronaut>();
        oldAstronautScript.isActive = false;
        Astronaut newAstronautScript = Astronauts[nextPlayerAtTurn-1].GetComponent<Astronaut>();
        newAstronautScript.ActivateAstronaut();

        PlayerAtTurn = nextPlayerAtTurn;
    }
}
