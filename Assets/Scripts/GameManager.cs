using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int PlayerAtTurn = 1; // obsolete
    public int NumberTeamsPlaying = 2;
    public int NumberAstronautsPerTeam = 3;
    public List<Astronaut> Astronauts; //obsolete
    public int ActiveTeam = 1;
    public int[] ActiveAstronautPerTeam = new int[4] { 1, -1, -1, -1 };
    public List<Astronaut> TurnHistory;
    public List<Astronaut> TeamA;
    public List<Astronaut> TeamB;
    public List<Astronaut> TeamC;
    public List<Astronaut> TeamD;
    private List<List<Astronaut>> TeamsAll = new List<List<Astronaut>>();
    public bool[] TeamsAlive = new bool[4];


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
        //Set TeamsAlive[4] initial according to NumberTeamsPlaying
        for(int i = 0; i < TeamsAlive.Length; i++)
        {
            if (i < NumberTeamsPlaying)
            {
                TeamsAlive[i] = true;
            } else
            {
                TeamsAlive[i] = false;
            }
        }
        TeamsAll.Add(TeamA);
        TeamsAll.Add(TeamB);
        TeamsAll.Add(TeamC);
        TeamsAll.Add(TeamD);

        //TurnHistory.Add(TeamA[0]);
        TransferAcitveAstronaut(TeamsAll[1][0], TeamsAll[0][0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandOverTurn()
    {
        
        ActiveTeam = GetNextValidTeam();
        Debug.Log("(next) Active Team" + ActiveTeam);
        if(ActiveTeam == -1) { GameOver(); return; }
        //Debug.Log(nextActiveTeam);
        int activeAstronaut = GetNextValidAstronaut(ActiveTeam);
        Astronaut nextAstronaut = TeamsAll[ActiveTeam-1][activeAstronaut-1];
        Debug.Log(nextAstronaut);
        TransferAcitveAstronaut(TurnHistory[TurnHistory.Count-1], nextAstronaut);

        ActiveAstronautPerTeam[ActiveTeam-1] = activeAstronaut;
        /*
        int nextPlayerAtTurn = PlayerAtTurn + 1;
        if (PlayerAtTurn > Astronauts.Count-1)
        {
            nextPlayerAtTurn = 1;
        }
        Debug.Log("Turn Over -> " + nextPlayerAtTurn);

        TransferAcitveAstronaut(Astronauts[PlayerAtTurn - 1], Astronauts[nextPlayerAtTurn - 1]);

        PlayerAtTurn = nextPlayerAtTurn;*/
    }

    /*
    private List<Astronaut> GetTeamReference(int TeamNo)
    {
        switch (ActiveTeam)
        {
            case 1:
                return TeamA;
            case 2:
                return TeamB;
            case 3:
                return TeamC;
            case 4:
                return TeamD;
        }
        Debug.LogError("THIS SHOULD NEVER APPEAR");
        return TeamA;
    }*/

    private int GetNextValidTeam()
    {
        int nextActiveTeam = ActiveTeam;
        bool validNextTeam = false;
        while (validNextTeam == false)
        {
            nextActiveTeam++;
            //Chack Out Of bounds
            if (nextActiveTeam > NumberTeamsPlaying)
            {
                nextActiveTeam = 1;
            }
            //Check if cycle back to same Team--> all others game Over
            if (nextActiveTeam == ActiveTeam) { return -1; }
            //Check still alive
            if (TeamsAlive[nextActiveTeam-1] == true) { validNextTeam = true; }
        }
        return nextActiveTeam;
    }

    private int GetNextValidAstronaut(int TeamNo)
    {
        int lastActiveAstronaut = ActiveAstronautPerTeam[ActiveTeam - 1];
        //if -1, this is the Teams first turn, Astronaut #1 should begin
        if (lastActiveAstronaut == -1) { return 1; }
        int nextActiveAstronaut = lastActiveAstronaut;
        bool validNextAstronaut = false;
        while (validNextAstronaut == false)
        {
            nextActiveAstronaut++;
            //Check out of Bounds
            if (nextActiveAstronaut > NumberAstronautsPerTeam) { nextActiveAstronaut = 1; }
            //Check still Alive
            if(TeamsAll[TeamNo-1][nextActiveAstronaut-1].isAlive) {
                Debug.Log("TeamNo: " + TeamNo + " nextActiveAstronaut: " + nextActiveAstronaut + " declared as alive");
                validNextAstronaut = true;
            }
            //check if cycle back to same & same is NOT alive --> entire team is dead
            if(nextActiveAstronaut == lastActiveAstronaut && !validNextAstronaut) { return -4514; }
        }
        return nextActiveAstronaut;
    }

    public void CheckTeamExtinction(int TeamNo)
    {
        //Debug.Log("CheckTeamExtinction");
        int nextMate = GetNextValidAstronaut(TeamNo);
        //Debug.Log("NextMate:" + nextMate);
        if (nextMate == -4514)
        {
            //Team dead!
            TeamsAlive[TeamNo - 1] = false;
        }
    }

    public void TransferAcitveAstronaut(Astronaut OldAstronaut, Astronaut NewAstronaut)
    {
        OldAstronaut.DeactivateAstronaut();
        NewAstronaut.ActivateAstronaut();
        TurnHistory.Add(NewAstronaut);
    }

    private void GameOver()
    {
        Debug.LogWarning("Game is Over. Somebody has won, there is nothing to see here.");
    }
}
