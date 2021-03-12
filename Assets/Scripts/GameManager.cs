using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    //public int PlayerAtTurn = 1; // obsolete
    [Tooltip("Should get set to player count from AppMgr")]
    public int NumberTeamsPlaying = 2;
    [Tooltip("Set to Astreonaut count")]
    public int NumberAstronautsPerTeam = 3;
    [Tooltip("LEAVE AT 4")]
    public int ActiveTeam = 1;
    [Tooltip("Leave at 0,0,0,0")]
    public bool[] TeamsAlive = new bool[4];
    [Tooltip("LEAVE AT 0,0,0,0")]
    public int[] ActiveAstronautPerTeam = new int[4] { 1, 0, 0, 0 };
    [Tooltip("LEAVE blank")]
    public List<Astronaut> TurnHistory;

    [Header("Fill this:")]
    [Tooltip("Fill with Astronauts from Team A")]
    public List<Astronaut> TeamA;
    [Tooltip("Fill with Astronauts from Team B")]
    public List<Astronaut> TeamB;
    [Tooltip("Fill with Astronauts from Team C")]
    public List<Astronaut> TeamC;
    [Tooltip("Fill with Astronauts from Team D")]
    public List<Astronaut> TeamD;

    public int maxTurnTime = 15;
    private double turnTimer;

    [Header("Internals, Public for accessability:")]
    [Tooltip("LEAVE TRUE")]
    public bool IsGameplayActive = true;
    private List<List<Astronaut>> TeamsAll = new List<List<Astronaut>>();

    [Header("References")]
    private Text TurnTimeText;
    private MenuIngame menuInGame;
    private MenuTurnStart menuTurnStart;
    private MenuGameOver menuGameOver;
    private UIProjectileSelection uIProjectileSelection;

    bool isSoundPlayed;
    private bool[] TeamsNeedLegend = new bool[4];

    private void Awake()
    {
        /*
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }
        */
        Instance = this;
        NumberTeamsPlaying = AppManager.Instance.SelectedPlayerCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnTimeText = CoolFunctions.FindInArray("TurnTimeText", GameObject.FindGameObjectsWithTag("UIReferences")).GetComponent<Text>();
        menuInGame = Resources.FindObjectsOfTypeAll<MenuIngame>()[0];
        menuTurnStart = Resources.FindObjectsOfTypeAll<MenuTurnStart>()[0];
        menuGameOver = Resources.FindObjectsOfTypeAll<MenuGameOver>()[0];
        uIProjectileSelection = CoolFunctions.FindInArray("ProjectileSelectionContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).transform.GetComponent<UIProjectileSelection>();

        InitializePlayerCount();

        //Set TeamsAlive[4] initial according to NumberTeamsPlaying
        for (int i = 0; i < TeamsAlive.Length; i++)
        {
            TeamsNeedLegend[i] = true;
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

        TurnHistory.Add(TeamA[0]);
        //TeamA[0].ActivateAstronaut();
        //TransferAcitveAstronaut(null, TeamA[0]); //Does not work

        isSoundPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTurnTime();
    }

    public void SetGameplayPause(bool isPause)
    {
        if (isPause)
        {
            IsGameplayActive = false;
            Time.timeScale = 0f;
        }
        else
        {
            IsGameplayActive = true;
            Time.timeScale = 1f;
        }
    }

    private void InitializePlayerCount()
    {
        if (NumberTeamsPlaying < 4)
        {
            for (int i = NumberAstronautsPerTeam-1; i >= 0; i--)
            {
                GravityManager.Instance.RemoveAnyObjectFromGravity(TeamD[i].gameObject);
                Destroy(TeamD[i].gameObject);
            }
            TeamD.Clear();
        }
        if (NumberTeamsPlaying < 3)
        {
            for (int i = NumberAstronautsPerTeam - 1; i >= 0; i--)
            {
                GravityManager.Instance.RemoveAnyObjectFromGravity(TeamC[i].gameObject);
                Destroy(TeamC[i].gameObject);
            }
            TeamC.Clear();
        }
    }

    private void CheckTurnTime()
    {
        //Debug.LogWarning("TurnHist Size" + TurnHistory.Count);
        Astronaut currentAstronautScript = TurnHistory[TurnHistory.Count - 1].GetComponent<Astronaut>();
        // During Flying Shot
        if (currentAstronautScript.shotFlying)
        {
            TurnTimeText.gameObject.GetComponent<Animator>().SetBool("IsAlert", false);
            TurnTimeText.text = "";
            return;
        }

        if (turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
            int intTurnTime = Mathf.RoundToInt((float)turnTimer);
            TurnTimeText.text = intTurnTime.ToString();
            if (intTurnTime <= 5)
            {
                TurnTimeText.gameObject.GetComponent<Animator>().SetBool("IsAlert", true);
                TimerRing(intTurnTime);
            }
        }
        else
        {
            TurnTimeText.gameObject.GetComponent<Animator>().SetBool("IsAlert", false);
            currentAstronautScript.EndShootingPhase();
        }
    }

    public void HandOverTurn()
    {
        ActiveTeam = GetNextValidTeam();
        //Debug.Log("(next) Active Team" + ActiveTeam);
        if(ActiveTeam == -1) { GameOver(); return; }
        //Debug.Log(nextActiveTeam);
        int activeAstronaut = GetNextValidAstronaut(ActiveTeam);
        Astronaut nextAstronaut = TeamsAll[ActiveTeam-1][activeAstronaut-1];
        //Debug.Log(nextAstronaut);
        TransferAcitveAstronaut(TurnHistory[TurnHistory.Count-1], nextAstronaut);

        ActiveAstronautPerTeam[ActiveTeam-1] = activeAstronaut;
        turnTimer = (double)maxTurnTime;

        //Activate TurnStart Window
        SetGameplayPause(true);
        menuTurnStart.gameObject.SetActive(true);
        menuTurnStart.SelectTurnStartScreen(ActiveTeam);

        
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

    public void ShowLegend()
    {
        if (TeamsNeedLegend[ActiveTeam - 1])
        {
            TeamsNeedLegend[ActiveTeam - 1] = false;
            SetGameplayPause(true);
            menuInGame.ShowLegend();
        }
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
        //if 0, this is the Teams first turn, Astronaut #1 should begin, if alive
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
        if (OldAstronaut != null)
            OldAstronaut.DeactivateAstronaut();
        NewAstronaut.ActivateAstronaut();
        //if this is astronauts first turn show weapon swap explanation
        /*if (!TurnHistory.Contains(NewAstronaut))
            uIProjectileSelection.ShowExplanation(4f);*/

        TurnHistory.Add(NewAstronaut);
        if (OldAstronaut != null)
            ActiveAstronautPerTeam[ActiveTeam - 1] = TeamsAll[ActiveTeam - 1].FindIndex(a => a == NewAstronaut) + 1;
        //Debug.Log("Added Astronaut #" + (TeamsAll[ActiveTeam - 1].FindIndex(a => a == NewAstronaut) + 1) + " to ActiveAstronautPerTeam");

        //CameraManager.Instance.GetComponent<Transform>().position.Set(NewAstronaut.transform.position.x, NewAstronaut.transform.position.y, CameraManager.Instance.stdCameraPosition.z);
        CameraManager.Instance.EnableTargetNZoom(NewAstronaut.transform);
        //CameraManager.Instance.EnableZoom();
        StartCoroutine(ResetCam());
    }

    IEnumerator ResetCam()
    {
        yield return new WaitForSeconds(1f);
        CameraManager.Instance.EnableTarget(transform);
        CameraManager.Instance.DisableZoom();
        yield return new WaitForSeconds(1.5f);
        //CameraManager.Instance.DisableZoom();
        CameraManager.Instance.DisableTarget();
        CameraManager.Instance.ResetPos();
    }

    private void GameOver()
    {
        Debug.LogWarning("Game is Over. Somebody has won, there is nothing to see here.");
        int winningTeam = 1;
        for(int i=1; i <= TeamsAlive.Length; i++)
        {
            if (TeamsAlive[i - 1])
                winningTeam = i;
        }
        StartCoroutine(DelayGameplayPause(winningTeam));
        menuInGame.gameObject.SetActive(false);
        menuGameOver.gameObject.SetActive(true);
    }

    IEnumerator DelayGameplayPause(int winningTeam)
    {
        yield return new WaitForSeconds(0.5f);
        SetGameplayPause(true);
        menuGameOver.SelectWinScreen(winningTeam);
        Debug.Log("Pasue Gameplay 2s after Game over to let Camera Zoom finish");
    }

    private void TimerRing(int intTurnTime)
    {
        if (intTurnTime == 5)
        {

            if (!isSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("timer");
                isSoundPlayed = true;
            }
            return;
        }
        if (intTurnTime == 4)
        {
            if (isSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("timer");
                isSoundPlayed = false;
            }
            return;
        }
        if (intTurnTime == 3)
        {
            if (!isSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("timer");
                isSoundPlayed = true;
            }
            return;
        }
        if (intTurnTime == 2)
        {
            if (isSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("timer");
                isSoundPlayed = false;
            }
            return;
        }
        if (intTurnTime == 1)
        {
            if (!isSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("timer");
                isSoundPlayed = true;
            }
            return;
        }
        if (intTurnTime == 0)
        {
            if (isSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("timer");
                isSoundPlayed = false;
            }
            return;
        }
    }
}
