using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // because Text

public class Astronaut : MonoBehaviour
{
    

    [Header("Association/Initials:")]
    public int TeamNo = 1;
    public int AstronautNo = 1;
    [Tooltip("Only set active initially on Astronaut A1")]
    public bool isActive = false;

    [Header("Constants")]
    public float movementSpeed = 1f;
    public float maxSpeed = 5f;
    public double sensitivityShotAngle = 0.03d;
    public double sensitivityShotAngle2 = 0.008d;
    public double sensitivityShotPower = 0.04d;
    public double sensitivityShotPower2 = 0.01d;
    [Header("Variables")]
    public int health = 100;
    public bool isAlive = true;
    public bool shootPhase = false;
    public bool shotFlying = false;

    public bool isStationary = true;
    public bool initializeTurn = true;
    public GameObject nearestPlanet;

    [Header("Private References")]
    private Rigidbody2D rBody;
    private UIShotBar uIshotBar;
    private Text uIshotText;
    private UIActiveIndicator uIActiveIndicator;
    private UILifeBars uILifeBars;
    private PostProcessing postProcessingScript;

    //private float RotateSpeed = 5f;
    //private float Radius = 0.1f;

    //private Vector2 _centre;
    //private float _angle;


    // private Vector3 inputVector;//up axis always being equal to the Y axis



    private int shotAngle = 0;
    private int shotPower = 50;


    private float horizontalMove;
    private float horizontalMoveAcceleration = 5;

    //for ProjectileCreation
    private ProjectileCreator projectileCreatorScript;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        nearestPlanet = GetNearestPlanet();
        //Debug.Log(nearestPlanet);

        //for ProjectileCreation
        projectileCreatorScript = GetComponentInChildren<ProjectileCreator>();

        uIshotBar = CoolFunctions.FindInArray("ShotBarContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).GetComponent<UIShotBar>();
        uIshotText = CoolFunctions.FindInArray("ShotTextContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).transform.GetComponent<Text>();
        uIActiveIndicator = CoolFunctions.FindInArray("ActiveIndicatorContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).transform.GetComponent<UIActiveIndicator>();

        uILifeBars = GameObject.FindGameObjectWithTag("ControllerLifeBars").GetComponent<UILifeBars>();

        //_centre = transform.position;
        postProcessingScript = GameObject.Find("PostProcessingVolume").GetComponent<PostProcessing>();
    }

    private void FixedUpdate()
    {
        if (!this.gameObject.activeSelf) return;

        if (initializeTurn) { InitializeTurnFixed(); }

        if (isActive)
        {
            if (!shootPhase)
            {
                AstronautMoverFixed();
            }
            else if (shootPhase)
            {
                //AstronautShooterFixed();
            }
        }


        if (isStationary)
        {
            TurnUpright();
        }
        else
        {

        }


        //ALWAYS
        //When Astronaut is too fast, limit its velocity/speed
        if (rBody.velocity.magnitude > maxSpeed)
        {
            rBody.velocity = Vector2.ClampMagnitude(rBody.velocity, maxSpeed);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (!shootPhase)
            {
                AstronautMover();
            }
            else if (shootPhase)
            {
                AstronautShooter();
            }
        }
    }

    // ----------------------------------------        Personalized Methods         -------------------------------------------

    private void InitializeTurnFixed()
    {
        /*uIActiveIndicator.Activate();
        uIActiveIndicator.SelectColor(TeamNo);*/
        initializeTurn = false;
    }

    private void AstronautMoverFixed()
    {   
        Vector3 localVelocity = transform.InverseTransformDirection(rBody.velocity);
        localVelocity.x = horizontalMove * horizontalMoveAcceleration;
        rBody.velocity = transform.TransformDirection(localVelocity);
    }

    private void AstronautMover()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (InputManager.Instance.Inputs.Contains("space"))
        {
            InputManager.Instance.Inputs.Remove("space");

            rBody.constraints = RigidbodyConstraints2D.FreezeAll;

            uIshotBar.Activate();
            uIshotBar.SetPower(shotPower);
            uIshotBar.SetAngle(shotAngle);
            uIshotText.gameObject.SetActive(true);
            uIshotText.text = shotAngle + ", " + shotPower;

            PredictionManager.Instance.lineRenderer.enabled = true;

            shootPhase = true;
        }
        else
        {
            horizontalMove = Input.GetAxis("Horizontal");
        }
    }

    private void AstronautShooterFixed()
    {
        if (shotFlying) return;
        


    }

    private double moveInputTimer = 0;
    private int moveUnitCount = 0;
    private void AstronautShooter()
    {
        if (shotFlying) return;

        if (InputManager.Instance.Inputs.Contains("space"))
        {
            InputManager.Instance.Inputs.Remove("space");
            

            Vector2 launchPosition = rBody.transform.position /*+ rBody.transform.up * 2.5f*/;

            
            projectileCreatorScript.ShootProjectile(launchPosition, shotAngle, shotPower, this.gameObject);

            uIshotBar.Deactivate();
            PredictionManager.Instance.lineRenderer.enabled = false;
            uIshotText.gameObject.SetActive(false);

            postProcessingScript.vignette.active = true;
            postProcessingScript.filmGrain.active = true;

            PredictionManager.Instance.lineRenderer.enabled = false;

            shotFlying = true;
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            shootPhase = false;
            uIshotBar.Deactivate();
            PredictionManager.Instance.lineRenderer.enabled = false;
            uIshotText.gameObject.SetActive(false);
            rBody.constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            Vector2 launchPosition = rBody.transform.position /*+ rBody.transform.up * 2.5f*/;

            projectileCreatorScript.SimulateProjectile(launchPosition, shotAngle, shotPower, this.gameObject);
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveInputTimer -= Time.deltaTime;
            if (moveInputTimer < 0)
            {
                moveUnitCount++;
                if (moveUnitCount <= 10)
                    moveInputTimer = sensitivityShotPower;
                else
                    moveInputTimer = sensitivityShotPower2;

                if (shotPower < 100)
                {
                    shotPower++;
                    uIshotBar.SetPower(shotPower);
                    uIshotText.text = shotAngle + ", " + shotPower;
                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveInputTimer -= Time.deltaTime;
            if (moveInputTimer < 0)
            {
                moveUnitCount++;
                if (moveUnitCount <= 10)
                    moveInputTimer = sensitivityShotPower;
                else
                    moveInputTimer = sensitivityShotPower2;

                if (shotPower > 0)
                {
                    shotPower--;
                    uIshotBar.SetPower(shotPower);
                    uIshotText.text = shotAngle + ", " + shotPower;
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInputTimer -= Time.deltaTime;
            if (moveInputTimer < 0)
            {
                moveUnitCount++;
                if (moveUnitCount <= 10)
                    moveInputTimer = sensitivityShotPower;
                else
                    moveInputTimer = sensitivityShotPower2;

                shotAngle--;
                if (shotAngle < 0) { shotAngle = 359; }
                uIshotBar.SetAngle(shotAngle);
                uIshotText.text = shotAngle + ", " + shotPower;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInputTimer -= Time.deltaTime;
            if (moveInputTimer < 0)
            {
                moveUnitCount++;
                if (moveUnitCount <= 10)
                    moveInputTimer = sensitivityShotPower;
                else
                    moveInputTimer = sensitivityShotPower2;


                shotAngle++;
                if (shotAngle > 359) { shotAngle = 0; }
                uIshotBar.SetAngle(shotAngle);
                uIshotText.text = shotAngle + ", " + shotPower;
            }
        }
        else
        {
            moveInputTimer = 0;
            moveUnitCount = 0;
        }
    }

    public void EndShootingPhase(float delay)
    {
        uIActiveIndicator.Deactivate();
        postProcessingScript.vignette.active = false;
        postProcessingScript.filmGrain.active = false;
        shotFlying = false;
        shootPhase = false;
        isActive = false;
        initializeTurn = true;
        GameManager.Instance.HandOverTurn(delay);
    }





    private GameObject GetNearestPlanet()
    {
        float nearestDistance = 0f;
        GameObject nearestPlanet = null;
        List<GameObject> planets = GravityManager.Instance.Planets;
        Vector2 positionAstronaut = this.transform.position;
        foreach(GameObject planet in planets)
        {
            Vector2 positionPlanet = planet.transform.position;
            Vector2 route = positionPlanet - positionAstronaut;
            float newDistance = route.magnitude;
            if (nearestDistance == 0f || newDistance <= nearestDistance)
            {
                nearestPlanet = planet;
                nearestDistance = newDistance;
            }
        }
        return nearestPlanet;
    }

    private void TurnUpright()
    {
        GameObject planet = nearestPlanet;
        Vector2 positionPlanet = planet.transform.position;
        Vector2 positionAstronaut = this.transform.position;
        Vector2 route = positionPlanet - positionAstronaut;
        float angle = Vector2.SignedAngle(new Vector2(0,100), route);
        rBody.transform.rotation = Quaternion.Euler(0,0,180f+angle); // +180 weil mit den Füßen statt mit Kopf richtung Planet
    }

    public void Damage(int Damage)
    {
        health = Mathf.Max(health - Damage, 0);
        uILifeBars.SetHealthValueOf(TeamNo, AstronautNo, health);
        if (health <= 0) {
            uILifeBars.DeactivateHealthBar(TeamNo, AstronautNo);
            DespawnAstronaut(); 
        }
    }

    private void DespawnAstronaut()
    {
        isAlive = false;
        Debug.Log("Team " + TeamNo + "Astronaut" + this.transform.name + " set dead");
        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        GameManager.Instance.CheckTeamExtinction(TeamNo);
        this.gameObject.SetActive(false); //read from gameObject.activeSelf
        //Destroy(this.gameObject);
    }

    public void ActivateAstronaut()
    {
        isActive = true;
        if(rBody == null) { rBody = GetComponent<Rigidbody2D>(); }
        rBody.constraints = RigidbodyConstraints2D.None;
        uIActiveIndicator.Activate();
        uIActiveIndicator.SelectColor(TeamNo);
    }

    public void DeactivateAstronaut()
    {
        isActive = false;
        SetStationary(true);
        uIshotBar.Deactivate();
        PredictionManager.Instance.lineRenderer.enabled = false;
    }

    public void SetStationary(bool val)
    {
        if (val == true)
        {
            isStationary = true;
            if (!isActive)
                rBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (val == false)
        {
            //to be determined
            //probably called when hit by launching missile o.Ä.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isStationary)
        {
            // especially for beginning when spawning to get stationary upon hitting the ground
            if (collision.collider.tag == "Ground")
                SetStationary(true);
        }
        else
        {
            // when walking do not glitch up other Astronauts
            /*does not work!
            if (collision.collider.tag == "Astronaut")
            {
                Debug.Log("collide with astronaut");
                rBody.velocity = Vector2.zero;
            }
            */
        }


    }
}
