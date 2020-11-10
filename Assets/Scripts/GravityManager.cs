using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance = null;

    public float GravityScale = 1f;
    public double dragConstant = 0.5d;
    public List<GameObject> Planets = new List<GameObject>();
    public List<GameObject> Astronauts = new List<GameObject>();
    public List<GameObject> Projectiles = new List<GameObject>();

    private List<Vector2> posPlanets = new List<Vector2>();
    private List<Rigidbody2D> rBodyAstronauts = new List<Rigidbody2D>();
    private List<Astronaut> scriptAstronauts = new List<Astronaut>();
    private List<Rigidbody2D> rBodyProjectiles = new List<Rigidbody2D>();

    // Awake is called before all Start()
    private void Awake()
    {
        if(Instance == null)
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
        foreach (GameObject p in Planets)
        {
            Vector2 v = p.transform.position;
            posPlanets.Add(v); //Vector3 is implicitly converted to Vector2 (z is discarded)
        }
        foreach (GameObject e in Astronauts)
        {
            Rigidbody2D rBody = e.GetComponent<Rigidbody2D>();
            rBodyAstronauts.Add(rBody);
            Astronaut script = e.GetComponent<Astronaut>();
            scriptAstronauts.Add(script);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate is by default executed 50Hz
    private void FixedUpdate()
    {
        for(int i=0; i<rBodyAstronauts.Count; i++)
        //foreach(Rigidbody2D astronaut in rBodyAstronauts)
        {
            Rigidbody2D astronaut = rBodyAstronauts[i];
            if (!scriptAstronauts[i].isStationary) {
                foreach (Vector2 posPlanet in posPlanets)
                {
                    //Vector2 direction = (Vector2)p.transform.position - e.position;
                    ApplyGravity(astronaut, posPlanet);
                    //Debug.Log(direction);
                    //Debug.Log(rBody);
                }
            } 
            else
            {
                ApplyGravity(astronaut, scriptAstronauts[i].nearestPlanet.transform.position);
            }
            ApplyDrag(astronaut);
        }

        foreach(Rigidbody2D projectile in rBodyProjectiles)
        {
            foreach(Vector2 posPlanet in posPlanets)
            {
                ApplyGravity(projectile, posPlanet);
            }
            ApplyDrag(projectile);
        }
    }

    private void ApplyGravity(Rigidbody2D EntityRBody, Vector2 posPlanet)
    {
        Vector2 direction = posPlanet - EntityRBody.position;
        float distance = direction.magnitude;
        Vector2 normatedDir = direction / distance;
        //float distanceFactor = (float) (1d / Math.Pow(distance - Planet.transform.localScale.x, 2f));
        float distanceFactor = (float)(1d / Math.Pow(distance, 2f)) * 10f;
        //Debug.Log(distanceFactor);
        Vector2 newGravity = normatedDir * (9.81f / 10f) * GravityScale * distanceFactor; // Gravity / FixedUPdate() called 50x a Second
        EntityRBody.AddForce(newGravity);
        //Debug.Log(newGravity);
    }

    private void ApplyDrag(Rigidbody2D rBody)
    {
        double velX = rBody.velocity.x;
        double velY = rBody.velocity.y;

        double dragX = Math.Min(Math.Abs(velX), dragConstant);
        double dragY = Math.Min(Math.Abs(velY), dragConstant);

        if (velX > 0)
        {
            dragX = -dragX;
        }
        if (velY > 0)
        {
            dragY = -dragY;
        }

        rBody.AddForce(new Vector2((float)dragX,(float)dragY));
    }



    // all public Interfacing methods

    public void AddProjectileToGravity(GameObject newProjectile)
    {
        Projectiles.Add(newProjectile);

        rBodyProjectiles.Add(newProjectile.GetComponent<Rigidbody2D>());
    }

    public void RemoveAnyObjectFromGravity(GameObject ObjectToRemove)
    {
        Projectiles.Remove(ObjectToRemove);
        Astronauts.Remove(ObjectToRemove);

        rBodyProjectiles.Remove(ObjectToRemove.GetComponent<Rigidbody2D>());
        rBodyAstronauts.Remove(ObjectToRemove.GetComponent<Rigidbody2D>());

    }
}
