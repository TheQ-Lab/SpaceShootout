using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public string projectileType = "";
    public int WeaponDamage = 50;
    public float lifetime;
    public GameObject explosionEffect;
    public GameObject missileEffect;
    


    private Rigidbody2D rBody;

    private float deathTimer;
    private GameObject parentAstronaut;
    private GameObject explosionBomb;
    private GameObject explosionMissile;

    bool explosionStarted = false;
    const float MissileExplosionDelay = 0.25f;
    const float BombExplosionDelay = 1.1f;
    float delayRemoveExplosion;

    //for Bomb
    float delay = 3f;
    float countdown;
    float radius = 5f;
    

    //for camera movement
    Transform camTarget;
    Transform camTrans;
    private Camera mainCamera;
    private Vector3 startCameraPosition;
    private float smoothSpeed = 0.125f;

    private Animator cameraAnimator;

    private bool isProjectileExistent = false;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        deathTimer = Time.time;
        countdown = delay;

        mainCamera = Camera.main;
        startCameraPosition = mainCamera.transform.position;
        camTrans = mainCamera.GetComponent<Transform>();
        cameraAnimator = mainCamera.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (explosionStarted)
        {
            Exploding();
        }
    }

    private void FixedUpdate()
    {
        if (Time.time >= deathTimer + lifetime) { DespawnThisProjectile(); }

        if (isProjectileExistent)
        {
            MoveCamera();
        }
    }

    private void Exploding()
    {
        if (projectileType.Equals("Missile"))
        {
            ExplodingMissile();
        }
        else
        {
            if (countdown <= 0f)
            {
                ExplodingBomb();
            }
            else
            {
                countdown -= Time.deltaTime;
            }
       }
    }

    private void ExplodingMissile()
    { 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D nearbyObject in colliders)
            {
                //Debug.Log(nearbyObject);
                if (nearbyObject.GetComponent<Astronaut>())
                {
                explosionMissile = Instantiate(missileEffect, transform.position, transform.rotation);
                Debug.Log("Do something, cause collider is with astronaut");
                    Astronaut victim = nearbyObject.GetComponent<Astronaut>();
                    victim.Damage(WeaponDamage);
                    break;
                }
            }
            DespawnThisProjectile();
    }

    private void ExplodingBomb()
    {
           
        //Debug.Log("explosion");
        Vector3 offset = new Vector3 (0, 0, -10);
        Vector3 explosionPos = transform.position + offset;
        explosionBomb = Instantiate(explosionEffect, explosionPos, Quaternion.LookRotation(rBody.velocity));
            
            // Get nearest Objects
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D nearbyObject in colliders)
            {
                //Debug.Log(nearbyObject);
                if (nearbyObject.GetComponent<Astronaut>())
                {
                    Debug.Log("Do something, cause collider is with astronaut");
                    Astronaut victim = nearbyObject.GetComponent<Astronaut>();
                    victim.Damage(WeaponDamage);
                    break;
                }
            } 
            DespawnThisProjectile();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        explosionStarted = true;
        if (collision.transform.name=="ProjectileBarrier" && projectileType.Equals("Bomb"))
        {
            countdown = 0f;
        }
    }

    public void SetInitialParameters(string _projectileType, Vector2 launchForce, float lifetime, GameObject parentAstronaut)
    {
        projectileType = _projectileType;   
        if (projectileType == "Missile")
        {
            delayRemoveExplosion = MissileExplosionDelay;
        }
        else
        {
            delayRemoveExplosion = BombExplosionDelay;
        }
        this.lifetime = lifetime;   
        float angle = Vector2.SignedAngle(new Vector2(0, 100), launchForce);
        rBody.transform.rotation = Quaternion.Euler(0, 0, angle);
        rBody.AddForce(launchForce);
        this.parentAstronaut = parentAstronaut;
        camTarget = this.GetComponent<Transform>();
        isProjectileExistent = true;
    }

    private void DespawnThisProjectile()
    {

        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        Astronaut parentAstronautScript = parentAstronaut.GetComponent<Astronaut>();
        parentAstronautScript.EndShootingPhase(delayRemoveExplosion);
        Destroy(this.gameObject);
       
        if (explosionBomb)
        {
            Destroy(explosionBomb, 2f);
        }
        else
        {
            Destroy(explosionMissile, 0.25f);
        }
        
        isProjectileExistent = false;   
    }

    void MoveCamera()
    {
        Vector3 offset = new Vector3(0, 0, -10);
        Vector3 desiredPosition = camTarget.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(camTrans.position, desiredPosition, smoothSpeed);
        camTrans.position = smoothPosition;
        cameraAnimator.SetBool("Zoomed In", true);
    }
}





