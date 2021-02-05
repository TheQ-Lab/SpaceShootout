using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance = null;

    public float GravityScale = 1f;
    public double dragConstant = 0.5d;
    [Tooltip("Fill this")]
    public List<GameObject> Planets = new List<GameObject>();
    public List<GameObject> Stars = new List<GameObject>();
    [Header("Can be left empty")]
    [Tooltip("Can be left empty; filled Trom Teams Lists in GameManager")]
    public List<GameObject> Astronauts = new List<GameObject>();
    [Tooltip("Can be left empty")]
    public List<GameObject> Projectiles = new List<GameObject>();

    private List<Rigidbody2D> rBodyPlanets = new List<Rigidbody2D>();
    private List<Rigidbody2D> rBodyStars = new List<Rigidbody2D>();
    private List<Rigidbody2D> rBodyAstronauts = new List<Rigidbody2D>();
    private List<Astronaut> scriptAstronauts = new List<Astronaut>();
    private List<Rigidbody2D> rBodyProjectiles = new List<Rigidbody2D>();

    // Awake is called before all Start()
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FillAstronautsFromGameMan();
        foreach (GameObject p in Planets)
        {
            Rigidbody2D v = p.GetComponent<Rigidbody2D>();
            rBodyPlanets.Add(v); //Vector3 is implicitly converted to Vector2 (z is discarded)
        }
        foreach (GameObject p in Stars)
        {
            Rigidbody2D v = p.GetComponent<Rigidbody2D>();
            rBodyStars.Add(v); //Vector3 is implicitly converted to Vector2 (z is discarded)
        }
        foreach (GameObject e in Astronauts)
        {
            Rigidbody2D rBody = e.GetComponent<Rigidbody2D>();
            rBodyAstronauts.Add(rBody);
            Astronaut script = e.GetComponent<Astronaut>();
            scriptAstronauts.Add(script);
        }
    }


    // FixedUpdate is by default executed 50Hz
    private void FixedUpdate()
    {
        for(int i=0; i<rBodyAstronauts.Count; i++)
        //foreach(Rigidbody2D astronaut in rBodyAstronauts)
        {
            Rigidbody2D astronaut = rBodyAstronauts[i];
            if (!scriptAstronauts[i].isStationary)
                ApplyGravityTowardsAllPlanets(astronaut, false);
            else
                ApplyGravity(astronaut, scriptAstronauts[i].nearestPlanet.GetComponent<Rigidbody2D>(), false);
            ApplyDrag(astronaut);
        }

        foreach(Rigidbody2D projectile in rBodyProjectiles)
        {
            ApplyGravityTowardsAllPlanets(projectile, true);
            //ApplyDrag(projectile);
        }
    }

    public void ApplyGravityTowardsAllPlanets(Rigidbody2D subject, bool withStars)
    {
        foreach (Rigidbody2D posPlanet in rBodyPlanets)
        {
            ApplyGravity(subject, posPlanet, true);
        }
        if (withStars)
            foreach (Rigidbody2D posStar in rBodyStars)
            {
                ApplyGravity(subject, posStar, false);
            }
    }

    private void ApplyGravity(Rigidbody2D EntityRBody, Rigidbody2D PlanetRBody, bool calculateFromSurface)
    {
        Vector2 direction = PlanetRBody.position - EntityRBody.position;
        float distance = direction.magnitude;
        Vector2 normatedDir = direction / distance;
        if (calculateFromSurface)
        {
            float rPlanet = PlanetRBody.GetComponent<CircleCollider2D>().radius;
            distance = distance - (rPlanet*0.8f); //if directly on ground -> bugs
        }
        //float distanceFactor = (float) (1d / Math.Pow(distance - Planet.transform.localScale.x, 2f));
        float distanceFactor = (float)(1d / Math.Pow(distance, 2f)) * 3f;
        float planetSize = (PlanetRBody.transform.localScale.x + PlanetRBody.transform.localScale.y) / 2f;
        Vector2 newGravity = normatedDir * (9.81f / 10f) * GravityScale * planetSize * distanceFactor; // Gravity / FixedUPdate() called 50x a Second
        EntityRBody.AddForce(newGravity);
        //Debug.Log(newGravity);
    }

    public void ApplyDrag(Rigidbody2D rBody)
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

    private void FillAstronautsFromGameMan()
    {
        List<Astronaut> teamSkriptList = GameManager.Instance.TeamA;
        teamSkriptList.AddRange(GameManager.Instance.TeamB);
        teamSkriptList.AddRange(GameManager.Instance.TeamC);
        teamSkriptList.AddRange(GameManager.Instance.TeamD);

        foreach (Astronaut s in teamSkriptList)
            Astronauts.Add(s.gameObject);
    }

    public void RemoveAnyObjectFromGravity(GameObject ObjectToRemove)
    {
        Projectiles.Remove(ObjectToRemove);
        Astronauts.Remove(ObjectToRemove);

        rBodyProjectiles.Remove(ObjectToRemove.GetComponent<Rigidbody2D>());
        rBodyAstronauts.Remove(ObjectToRemove.GetComponent<Rigidbody2D>());
        scriptAstronauts.Remove(ObjectToRemove.GetComponent<Astronaut>());

    }

    public bool checkCollision(GameObject obj)
    {
        foreach (GameObject collider in Planets)
        {
            Vector3 closestPoint = collider.GetComponent<Collider2D>().ClosestPoint(obj.transform.position);
            if (Vector3.Distance(closestPoint, obj.transform.position) < 0.3f || closestPoint == obj.transform.position) return true;
        }

        foreach (GameObject collider in Stars)
        {
            Vector3 closestPoint = collider.GetComponent<Collider2D>().ClosestPoint(obj.transform.position);
            if (Vector3.Distance(closestPoint, obj.transform.position) < 0.1f || closestPoint == obj.transform.position) return true;
        }
        foreach (GameObject collider in Astronauts)
        {
            Vector3 closestPoint = collider.GetComponent<Collider2D>().ClosestPoint(obj.transform.position);
            if (Vector3.Distance(closestPoint, obj.transform.position) < 0.1f || closestPoint == obj.transform.position) return true;
        }

        return false;
    }
}
