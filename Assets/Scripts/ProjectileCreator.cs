using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public GameObject MissilePrefab;

    private GameObject Missile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootProjectile(string projectileType, Vector2 spawnPosition, Vector2 launchVector)
    {
        Missile = Instantiate(MissilePrefab, spawnPosition, /*EXCHANGE*/Quaternion.identity);
        Missile.AddComponent<Projectile>();
        Projectile projectileScript = Missile.GetComponent<Projectile>();
        projectileScript.SetInitialParameters(projectileType, launchVector);
        GravityManager.Instance.AddProjectileToGravity(Missile);
    }
}
