using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType = "";
    public int WeaponDamage = 100;
    public float lifetime;
    public GameObject Planet;
    public float explosionStrength = 100;



    private Rigidbody2D rBody;
    private Rigidbody rb;

    private float deathTimer;
    private GameObject parentAstronaut;


    //for Bomb
    float delay = 3f;
    float countdown;

    bool hasExploded = false;

    public GameObject explosionEffect;
    public float radius = 5f;
    public float force = 700f;


    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        deathTimer = Time.time;
        countdown = delay;

    }

    // Update is called once per frame
    void Update()
    {



    }

    private void FixedUpdate()
    {
        if (Time.time >= deathTimer + lifetime) { DespawnThisProjectile(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Astronaut astronautScript = collision.gameObject.GetComponent<Astronaut>();
        

        //maybe for future update with planet
        //PlanetGrassland planetScript = collision.gameObject.GetComponent<PlanetGrassland>();

       
            
            if (projectileType.Equals("Missile"))
                
            {
                if (astronautScript != null)
                {
                    astronautScript.Damage(WeaponDamage);
                   
                }
                DespawnThisProjectile();
        }


        if (projectileType.Equals("Bomb"))
        {
            

            Explode();

        }

    }

    public void SetInitialParameters(string _projectileType, Vector2 launchForce, GameObject parentAstronaut)
    {
        projectileType = _projectileType;

        if (_projectileType.Equals("Bomb"))
        {
            lifetime = 5f;
        }
        else
        {
            lifetime = 5f;
        }

        //rBody.AddForce(launchForce);

        float angle = Vector2.SignedAngle(new Vector2(0, 100), launchForce);
        rBody.transform.rotation = Quaternion.Euler(0, 0, angle);

        rBody.AddForce(launchForce);

        this.parentAstronaut = parentAstronaut;
    }

    private void DespawnThisProjectile()
    {
        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        Astronaut parentAstronautScript = parentAstronaut.GetComponent<Astronaut>();
        parentAstronautScript.EndShootingPhase();
        Destroy(this.gameObject);
    }

    void Explode()
    {
        //Show effect
        Vector3 explosionPos = transform.position;

        // Get nearest Object
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        //Vector2 impulse = new Vector2(-700, 200);
        


        //Add force
        foreach (Collider2D nearbyObject in colliders)
        {
           
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            //rb.AddForce(impulse, ForceMode2D.Impulse);
            rb.AddExplosionForce(force, explosionPos, radius);
 
        }

        Destroy(rb);
        Debug.Log("destroyed");



        //Remove a bomb
        // Destroy(gameObject);
    }
    
}





