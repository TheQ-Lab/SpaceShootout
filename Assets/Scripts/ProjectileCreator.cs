using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public GameObject MissilePrefab;

    public float lifetime = 5f;

    private GameObject Missile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootProjectile(string projectileType, Vector2 spawnPosition, int shotAngle, int shotPower, GameObject parentAstronaut)
    {
        float powerFactor = 1600f; //for missile specific probably

        Vector2 directionalVector = Quaternion.Euler(0, 0, shotAngle) * Vector2.up;

        spawnPosition = spawnPosition + directionalVector * 2f;
        Vector2 launchVector = directionalVector * (shotPower * 0.01f) * powerFactor;

        Debug.Log(launchVector);


        Missile = Instantiate(MissilePrefab, spawnPosition, Quaternion.Euler(0,0,shotAngle));
        Missile.AddComponent<Projectile>();
        Projectile projectileScript = Missile.GetComponent<Projectile>();
        projectileScript.SetInitialParameters(projectileType, launchVector, lifetime, parentAstronaut);
        GravityManager.Instance.AddProjectileToGravity(Missile);
    }
}
