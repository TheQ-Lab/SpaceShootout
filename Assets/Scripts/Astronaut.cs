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
    private char facing = 'r';

    [Header("Private References")]
    private Rigidbody2D rBody;
    private Animator modelAnim;
    private UIShotBar uIshotBar;
    //private Text uIshotText;
    private UIShotStats uIShotStats;
    private UIActiveIndicator uIActiveIndicator;
    private UILifeBars uILifeBars;
    private PostProcessing postProcessingScript;

    //private float RotateSpeed = 5f;
    //private float Radius = 0.1f;

    //private Vector2 _centre;
    //private float _angle;


    // private Vector3 inputVector;//up axis always being equal to the Y axis

    float downAngle, aimAngle;

    private int shotAngle = 0;
    private int shotPower = 50;
    private bool sliderHasEventListener = false;

    private double moveInputTimer = 0;
    private int moveUnitCount = 0;


    private float horizontalMove;
    private float horizontalMoveAcceleration = 5;

    Slider mySlider;

    //for ProjectileCreation
    private ProjectileCreator projectileCreatorScript;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        modelAnim = CoolFunctions.FindChildWithTag(gameObject, "Model", true).GetComponent<Animator>();
        nearestPlanet = GetNearestPlanet();
        //Debug.Log(nearestPlanet);

        //for ProjectileCreation
        projectileCreatorScript = GetComponentInChildren<ProjectileCreator>();

        uIshotBar = CoolFunctions.FindInArray("ShotBarContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).GetComponent<UIShotBar>();
        uIShotStats = CoolFunctions.FindInArray("ShotStatsContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).transform.GetComponent<UIShotStats>();
        uIActiveIndicator = CoolFunctions.FindInArray("ActiveIndicatorContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).transform.GetComponent<UIActiveIndicator>();

        uILifeBars = GameObject.FindGameObjectWithTag("ControllerLifeBars").GetComponent<UILifeBars>();

        //_centre = transform.position;
        postProcessingScript = GameObject.Find("PostProcessingVolume").GetComponent<PostProcessing>();

        
        
    }

    private void FixedUpdate()
    {
        if (!this.gameObject.activeSelf) return;

        

        if (isActive)
        {
            if (initializeTurn) { InitializeTurnFixed(); }

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
        if (!GameManager.Instance.IsGameplayActive)
            return;

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
        //Debug.LogWarning(this.gameObject.name);
        projectileCreatorScript.InitializeTurn();
    }

    private void UpdateFacing(char n)
    {
        if (n == facing)
            return;
        if (n == 'r')
        {
            modelAnim.SetInteger("Facing", 1);
        }
        else if (n == 'l')
        {
            modelAnim.SetInteger("Facing", -1);
        }
        facing = n;
    }

    private void AstronautMoverFixed()
    {   
        Vector3 localVelocity = transform.InverseTransformDirection(rBody.velocity);
        localVelocity.x = horizontalMove * horizontalMoveAcceleration;
        rBody.velocity = transform.TransformDirection(localVelocity);
    }

    private void AstronautMover()
    {
        
        if (InputManager.Instance.Inputs.Contains("space"))
        {
            InputManager.Instance.Inputs.Remove("space");
            



            rBody.constraints = RigidbodyConstraints2D.FreezeAll;

            uIshotBar.Activate();
            uIshotBar.SetPower(shotPower);
            uIshotBar.SetAngle(shotAngle);
            uIShotStats.SetActive(true);
            uIShotStats.SetPower(shotPower);
            uIShotStats.SetAngle(shotAngle);
            if (!sliderHasEventListener)
            {
                sliderHasEventListener = true;
                uIshotBar.slider.onValueChanged.AddListener(delegate { SliderValueChangeCheck(); });
            }
            PredictionManager.Instance.lineRenderer.enabled = true;

            shootPhase = true;
        }
        else
        {
            horizontalMove = Input.GetAxis("Horizontal");
            if (horizontalMove > 0)
                UpdateFacing('r');
            else if (horizontalMove < 0)
                UpdateFacing('l');
        }
    }

    private void AstronautShooterFixed()
    {
        if (shotFlying) return;
        


    }

    
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
            uIShotStats.SetActive(false);

            postProcessingScript.vignette.active = true;
            postProcessingScript.filmGrain.active = true;

            PredictionManager.Instance.lineRenderer.enabled = false;

            shotFlying = true;
            InputManager.Instance.Inputs.Remove("arrow");
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            shootPhase = false;
            uIshotBar.Deactivate();
            PredictionManager.Instance.lineRenderer.enabled = false;
            uIShotStats.SetActive(false);
            rBody.constraints = RigidbodyConstraints2D.None;
            InputManager.Instance.Inputs.Remove("arrow");

        }
        else
        {
            Vector2 launchPosition = rBody.transform.position /*+ rBody.transform.up * 2.5f*/;

            projectileCreatorScript.SimulateProjectile(launchPosition, shotAngle, shotPower, this.gameObject);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
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
                    uIShotStats.SetPower(shotPower);
                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
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
                    uIShotStats.SetPower(shotPower);
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
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
                uIShotStats.SetAngle(shotAngle);

                /*
                aimAngle = shotAngle;
                if (aimAngle > 180f)
                {
                    aimAngle = shotAngle - 360f;
                }
                float upAngle = rBody.transform.rotation.eulerAngles.z;
                if (upAngle < 0f)
                    upAngle += 360f;
                downAngle = upAngle + 180f;
                if ((aimAngle > upAngle && aimAngle < downAngle) || (aimAngle > upAngle && aimAngle < downAngle - 360f))
                {
                    UpdateFacing('r');
                }*/
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
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
                uIShotStats.SetAngle(shotAngle);
            }
        }
        else if (InputManager.Instance.Inputs.Contains("arrow"))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 direction = uIshotBar.transform.rotation * Vector3.up;
            
            float angle = Vector3.SignedAngle(direction, mousePos - uIshotBar.transform.position, Vector3.forward);

            //Debug.Log(angle + " " + uIshotBar.transform.rotation.eulerAngles.z);
            if (angle < -90 || angle > 90)
            {
                shotAngle--;
                if (shotAngle < 0) { shotAngle = 359; }
                uIshotBar.SetAngle(shotAngle);
                uIShotStats.SetAngle(shotAngle);

            }
            else
            {
                shotAngle++;
                if (shotAngle > 359) { shotAngle = 0; }
                uIshotBar.SetAngle(shotAngle);
                uIShotStats.SetAngle(shotAngle);
            }
        }
        else
        {
            moveInputTimer = 0;
            moveUnitCount = 0;
            
        }

        //Face in direction of Aiming
        Vector2 differenceVector = Quaternion.Inverse(rBody.transform.rotation) * (Quaternion.Euler(0, 0, shotAngle) * Vector2.up);
        if (differenceVector.x > 0)
            UpdateFacing('r');
        else if (differenceVector.x < 0)
            UpdateFacing('l');
    }

    public void SliderValueChangeCheck()
    {
        //Debug.Log("slider changed" + uIshotBar.slider.value);
        shotPower = (int)(uIshotBar.slider.value / 0.01f);
    }

    public void EndShootingPhase()
    {
        uIActiveIndicator.Deactivate();
        postProcessingScript.vignette.active = false;
        postProcessingScript.filmGrain.active = false;
        shotFlying = false;
        shootPhase = false;
        isActive = false;
        initializeTurn = true;
        GameManager.Instance.HandOverTurn();
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
        ProjectileCreator.Instance = projectileCreatorScript;
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
