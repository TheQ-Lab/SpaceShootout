using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    [System.Serializable]
    public class ProjectileConfiguration
    {
        public Projectile.Type Type;
        public GameObject Prefab;
        public float Lifetime;

        ProjectileConfiguration(Projectile.Type type, GameObject prefab, float lifetime)
        {
            Type = type;
            Prefab = prefab;
            Lifetime = lifetime;
        }

        public static ProjectileConfiguration zero()
        {
            ProjectileConfiguration ret = new ProjectileConfiguration(Projectile.Type.Missile, null, 0f);
            return ret;
        }
    }


    public List<ProjectileConfiguration> projectileConfigurations;

    // REFERENCES
    private UIProjectileSelection uIProjectileSelection;

    private GameObject projectile;
    private ProjectileConfiguration currentProjectileConfig = ProjectileConfiguration.zero();

    // Start is called before the first frame update
    void Start()
    {
        uIProjectileSelection = CoolFunctions.FindInArray("ProjectileSelectionContainer", GameObject.FindGameObjectsWithTag("UIReferences")).transform.GetChild(0).transform.GetComponent<UIProjectileSelection>();

        SetCurrentProjectile(Projectile.Type.Missile);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponentInParent<Astronaut>().isActive)
            CheckWeaponChangeInputs();

        //ONLY TEMPORARY, projectileCreator is a MESS, Strings instead of enums, a FUCK ton of hard code instead of Serialized class and above all PERMANENT listening for projectileChange commands, regardless of isActive of the parent Astronaut!!!
        //nobody'll know where to change timings etc.
        
    }
   
    // +++Shooting + Simulation+++
    public void ShootProjectile(Vector2 spawnPosition, int shotAngle, int shotPower, GameObject parentAstronaut)
    {
        float powerFactor = 1600f; //for missile specific probably

        Vector2 directionalVector = Quaternion.Euler(0, 0, shotAngle) * Vector2.up;

        spawnPosition = spawnPosition + directionalVector * 2f;
        float clampedShotPower = 35f + shotPower * 0.5f;
        Vector2 launchVector = directionalVector * (clampedShotPower * 0.01f) * powerFactor;

        //Debug.Log(launchVector);

        projectile = Instantiate(currentProjectileConfig.Prefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));

        GravityManager.Instance.AddProjectileToGravity(projectile);
        projectile.GetComponent<Projectile>().launch(launchVector, currentProjectileConfig.Lifetime, parentAstronaut);
    }

    public void SimulateProjectile(Vector2 spawnPosition, int shotAngle, int shotPower, GameObject parentAstronaut)
    {
        float powerFactor = 1600f; //for missile specific probably

        Vector2 directionalVector = Quaternion.Euler(0, 0, shotAngle) * Vector2.up;

        spawnPosition = spawnPosition + directionalVector * 2f;
        float clampedShotPower = 35f + shotPower * 0.5f;
        Vector2 launchVector = directionalVector * (clampedShotPower * 0.01f) * powerFactor;

        projectile = Instantiate(currentProjectileConfig.Prefab, spawnPosition, Quaternion.Euler(0, 0, shotAngle));

        PredictionManager.Instance.predict(currentProjectileConfig.Prefab, spawnPosition, launchVector);
        Destroy(projectile);
    }


    // +++Weapon selection+++
    public void InitializeTurn()
    {
        SetCurrentProjectile(currentProjectileConfig.Type);
    }

    private void CheckWeaponChangeInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCurrentProjectile(Projectile.Type.Missile);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCurrentProjectile(Projectile.Type.Bomb);
        else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (currentProjectileConfig.Type == Projectile.Type.Missile)
                SetCurrentProjectile(Projectile.Type.Bomb);
            else if (currentProjectileConfig.Type == Projectile.Type.Bomb)
                SetCurrentProjectile(Projectile.Type.Missile);
        }
    }

    private void SetCurrentProjectile(Projectile.Type t)
    {
        currentProjectileConfig = GetProjectileConfig(t);

        // TEMP
        //if (transform.parent.GetComponent<Astronaut>().isActive)
        uIProjectileSelection.SelectIcon(t);
        //Debug.LogWarning("Select" + t.ToString());
    }

    private ProjectileConfiguration GetProjectileConfig(Projectile.Type t)
    {
        ProjectileConfiguration ret = ProjectileConfiguration.zero();
        foreach(ProjectileConfiguration c in projectileConfigurations)
        {
            if (c.Type.Equals(t))
            {
                ret = c;
            }
        }
        return ret;
    }
}
