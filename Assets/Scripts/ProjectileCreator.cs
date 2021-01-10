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
    public string currentProjectile;
    

    public float lifetime;

    private String[] projectiles = {"Bomb", "Missile"};

   

    // Start is called before the first frame update
    void Start()
    {
        SelectProjectile(selectedProjectile);
        currentProjectile = projectiles[selectedProjectile];

    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedProjectile = selectedProjectile;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedProjectile >= projectiles.Length - 1)
                selectedProjectile = 0;
            else
                selectedProjectile++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedProjectile <= 0)
                selectedProjectile = projectiles.Length - 1;
            else
                selectedProjectile--;
        }

        if (previousSelectedProjectile != selectedProjectile)
        {
            SelectProjectile(selectedProjectile);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedProjectile = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedProjectile = 1;

        currentProjectile = projectiles[selectedProjectile];

        if (selectedProjectile == 0)
            lifetime = 7f;
        else if (selectedProjectile == 1)
            lifetime = 10f;
        
    }
   
    public void ShootProjectile(Vector2 spawnPosition, int shotAngle, int shotPower, GameObject parentAstronaut)
    {
        float powerFactor = 1600f; //for missile specific probably

        Vector2 directionalVector = Quaternion.Euler(0, 0, shotAngle) * Vector2.up;

        spawnPosition = spawnPosition + directionalVector * 2f;
        float clampedShotPower = 35f + shotPower * 0.5f;
        Vector2 launchVector = directionalVector * (clampedShotPower * 0.01f) * powerFactor;

        //Debug.Log(launchVector);

        if (selectedProjectile == 0)
        {
            projectile = Instantiate(BombPrefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));
        }
        else if (selectedProjectile == 1)
        {
            projectile = Instantiate(MissilePrefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));
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

        void SelectProjectile(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
     
   

}
