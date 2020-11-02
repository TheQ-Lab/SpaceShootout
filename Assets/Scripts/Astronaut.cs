using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    public bool isActive = false;
    public float movementSpeed = 1f;
    public float maxSpeed = 5f;

    private Rigidbody2D rBody;

    private bool isStationary = true;
    private GameObject nearestPlanet;

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

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            AstronautMover();
            AstronautShooter();
        }
        if (isStationary)
        {
            TurnUpright();
        }
        //ALWAYS
        //When Astronaut is too fast, limit its velocity/speed
        if(rBody.velocity.magnitude > maxSpeed)
        {
            rBody.velocity = Vector2.ClampMagnitude(rBody.velocity, maxSpeed);
        }
        
    }

    private void AstronautMover()
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

    private void AstronautShooter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float launchStrength = 500f;

            Vector2 launchPosition = rBody.transform.position + rBody.transform.up * 1.5f;

            Vector2 launchForce = rBody.transform.up; //has alredy magnitude == 1
            launchForce = launchForce * launchStrength;
            projectileCreatorScript.ShootProjectile("Missile", launchPosition, launchForce);
        }
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
}
