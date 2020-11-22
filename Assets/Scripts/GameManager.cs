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
    public List<Astronaut> TurnHistory; //obsolete
    public List<Astronaut> TeamA;
    public List<Astronaut> TeamB;
    public List<Astronaut> TeamC;
    public List<Astronaut> TeamD;
    public bool[] TeamsAlive = new bool[4];

    //public List<List<GameObject>> AstronautsAlle;  // for later, when we have multiple Astronauts per Team use this 2-Dimensional List
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

        TurnHistory.Add(TeamA[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandOverTurn()
    {
        
        ActiveTeam = GetNextValidTeam();
        if(ActiveTeam == -1) { GameOver(); return; }
        List<Astronaut> nextActiveTeam = GetTeamReference(ActiveTeam);
        //Debug.Log(nextActiveTeam);
        int activeAstronaut = GetNextValidAstronaut(ActiveTeam);
        Astronaut nextAstronaut = nextActiveTeam[activeAstronaut-1];
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
        Debug.Log("HAHLTSTOPP");
        return TeamA;
    }

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
        List<Astronaut> Team = GetTeamReference(TeamNo);
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
            if(Team[nextActiveAstronaut-1].isAlive) { validNextAstronaut = true; }
            //check if cycle back to same & same is NOT alive --> entire team is dead
            if(nextActiveAstronaut == lastActiveAstronaut && !validNextAstronaut) { return -4514; }
        }
        return nextActiveAstronaut;
    }

    public void CheckTeamExtinction(int TeamNo)
    {
        int nextMate = GetNextValidAstronaut(TeamNo);
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
