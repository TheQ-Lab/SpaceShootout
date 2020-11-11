﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // because Text

public class Astronaut : MonoBehaviour
{
    public UIShotBar uIshotBar;
    public Text uIshotText;
    public UIActiveIndicator uIActiveIndicator;

    public int playerNo = 1;
    public bool isActive = false;
    public float movementSpeed = 1f;
    public float maxSpeed = 5f;
    public int health = 100;
    public bool shootPhase = false;
    public bool shotFlying = false;

    public bool isStationary = true;
    public bool initializeTurn = true;
    public GameObject nearestPlanet;

    private Rigidbody2D rBody;

    
    
    private int shotAngle = 0;
    private int shotPower = 50;
    private int timer = 0;

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
            } else if (shootPhase)
            {
                AstronautShooterFixed();
            }
        }


        if (isStationary)
        {
            TurnUpright();
        }
        //ALWAYS
        //When Astronaut is too fast, limit its velocity/speed
        if (rBody.velocity.magnitude > maxSpeed)
        {
            rBody.velocity = Vector2.ClampMagnitude(rBody.velocity, maxSpeed);
        }

        if (health <= 0)
        {
            DespawnAstronaut();
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
        uIActiveIndicator.Activate();
        uIActiveIndicator.SelectColor(playerNo);
        initializeTurn = false;
    }

    private void AstronautMoverFixed()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rBody.AddRelativeForce(new Vector2(movementSpeed, 0f));
        } 
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rBody.AddRelativeForce(new Vector2(-movementSpeed, 0f));
        }
    }

    private void AstronautMover()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (InputManager.Instance.Inputs.Contains("space"))
        {
            InputManager.Instance.Inputs.Remove("space");

            shootPhase = true;
            rBody.constraints = RigidbodyConstraints2D.FreezeAll;

            uIshotBar.Activate();
            uIshotBar.SetPower(shotPower);
            uIshotBar.SetAngle(shotAngle);
            uIshotText.gameObject.SetActive(true);
            uIshotText.text = shotAngle + ", " + shotPower;
        }
    }

    private void AstronautShooterFixed()
    {
        if (shotFlying) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            timer++;
            if (timer >= 2)
            {
                timer = 0;
                if (shotPower < 100) { shotPower++;
                    uIshotBar.SetPower(shotPower);
                    uIshotText.text = shotAngle + ", " + shotPower;
                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            timer++;
            if (timer >= 2)
            {
                timer = 0;
                if (shotPower > 0) { shotPower--;
                    uIshotBar.SetPower(shotPower);
                    uIshotText.text = shotAngle + ", " + shotPower;
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            timer++;
            if (timer >= 2)
            {
                timer = 0;
                shotAngle--;
                if (shotAngle < 0) { shotAngle = 359; }
                uIshotBar.SetAngle(shotAngle);
                uIshotText.text = shotAngle + ", " + shotPower;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            timer++;
            if (timer >= 2)
            {
                timer = 0;
                shotAngle++;
                if (shotAngle > 359) { shotAngle = 0; }
                uIshotBar.SetAngle(shotAngle);
                uIshotText.text = shotAngle + ", " + shotPower;
            }
        }
        else
        {
            timer = 0;
        }


    }

    private void AstronautShooter()
    {
        if (shotFlying) return;
        //if (Input.GetKeyDown(KeyCode.Space))
        if (InputManager.Instance.Inputs.Contains("space"))
        {
            InputManager.Instance.Inputs.Remove("space");
            

            Vector2 launchPosition = rBody.transform.position /*+ rBody.transform.up * 2.5f*/;

            
            projectileCreatorScript.ShootProjectile("Missile", launchPosition, shotAngle, shotPower, this.gameObject);

            uIshotBar.Deactivate();
            uIshotText.gameObject.SetActive(false);

            shotFlying = true;
        }
    }

    public void EndShootingPhase()
    {
        uIActiveIndicator.Deactivate();
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
    }

    private void DespawnAstronaut()
    {
        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        this.gameObject.SetActive(false); //read from gameObject.activeSelf
        //Destroy(this.gameObject);
    }

    public void ActivateAstronaut()
    {
        isActive = true;
        rBody.constraints = RigidbodyConstraints2D.None;
    }
}
