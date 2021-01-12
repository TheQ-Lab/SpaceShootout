using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public GameObject MissilePrefab;
    public GameObject BombPrefab;

    private GameObject projectile;
   
    public int selectedProjectile = 0;
    public Projectile.Type currentProjectile = Projectile.Type.Missile;
    

    public float lifetime;

    private String[] projectiles = {"Bomb", "Missile"};

   

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentProjectile(Projectile.Type.Missile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCurrentProjectile(Projectile.Type.Missile);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCurrentProjectile(Projectile.Type.Bomb);
        else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (currentProjectile == Projectile.Type.Missile)
                SetCurrentProjectile(Projectile.Type.Bomb);
            else if (currentProjectile == Projectile.Type.Bomb)
                SetCurrentProjectile(Projectile.Type.Missile);
        }

        //ONLY TEMPORARY, projectileCreator is a MESS, Strings instead of enums, a FUCK ton of hard code instead of Serialized class and above all PERMANENT listening for projectileChange commands, regardless of isActive of the parent Astronaut!!!
        //nobody'll know where to change timings etc.
        
    }
   
    public void ShootProjectile(Vector2 spawnPosition, int shotAngle, int shotPower, GameObject parentAstronaut)
    {
        float powerFactor = 1600f; //for missile specific probably

        Vector2 directionalVector = Quaternion.Euler(0, 0, shotAngle) * Vector2.up;

        spawnPosition = spawnPosition + directionalVector * 2f;
        float clampedShotPower = 35f + shotPower * 0.5f;
        Vector2 launchVector = directionalVector * (clampedShotPower * 0.01f) * powerFactor;

        //Debug.Log(launchVector);

        if (currentProjectile == Projectile.Type.Missile)
        {
            projectile = Instantiate(MissilePrefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));
        }
        else if (currentProjectile == Projectile.Type.Bomb)
        {
            projectile = Instantiate(BombPrefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));
        }
        GravityManager.Instance.AddProjectileToGravity(projectile);
        projectile.GetComponent<Projectile>().launch(launchVector, lifetime, parentAstronaut);
    }

    public void SimulateProjectile(Vector2 spawnPosition, int shotAngle, int shotPower, GameObject parentAstronaut)
    {
        float powerFactor = 1600f; //for missile specific probably

        Vector2 directionalVector = Quaternion.Euler(0, 0, shotAngle) * Vector2.up;

        spawnPosition = spawnPosition + directionalVector * 2f;
        float clampedShotPower = 35f + shotPower * 0.5f;
        Vector2 launchVector = directionalVector * (clampedShotPower * 0.01f) * powerFactor;

        projectile = Instantiate(MissilePrefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));

        PredictionManager.Instance.predict(MissilePrefab, spawnPosition, launchVector);
        Destroy(projectile);
    }

    private void SetCurrentProjectile(Projectile.Type t)
    {
        currentProjectile = t;

        if (t == Projectile.Type.Missile)
        {
            lifetime = 10f;
        }
        else if (t == Projectile.Type.Bomb)
        {
            lifetime = 7f;
        }


        if (transform.parent.GetComponent<Astronaut>().isActive)
        {
            transform.parent.GetComponent<Astronaut>().uIProjectileSelection.SelectIcon(t);
        }

    }



}
