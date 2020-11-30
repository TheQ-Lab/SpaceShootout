﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType = "";
    public int WeaponDamage = 50;
    public float lifetime;
   
    private Rigidbody2D rBody;

    private float deathTimer;
    private GameObject parentAstronaut;


    //for Bomb
    float delay = 3f;
    float countdown;
    bool explosion = false;
    bool timerStarted = false;
    float radius = 5f;

    //for camera movement
    Transform camTarget;
    Transform camTrans;
    private Camera mainCamera;
    private Vector3 startCameraPosition;
    float startCameraSize;
    private float smoothSpeed = 0.125f;

    private float targetZoom;
    private float zoomFactor = -0.15f;
    private float zoomLerpSpeed = 0.15f;

    private bool projectileExistiert = false;

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
        startCameraSize = mainCamera.orthographicSize;
        camTrans = mainCamera.GetComponent<Transform>();
        targetZoom = mainCamera.orthographicSize;

    }

    // Update is called once per frame
    void Update()
    {
        //Timer for explosion
        if (timerStarted)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                explosion = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time >= deathTimer + lifetime) { DespawnThisProjectile(); }

        if (projectileExistiert)
        {
            MoveCamera();
        }
       
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

    public void SetInitialParameters(string _projectileType, Vector2 launchForce, float lifetime, GameObject parentAstronaut)
    {
        projectileType = _projectileType;
        this.lifetime = lifetime;
       
        float angle = Vector2.SignedAngle(new Vector2(0, 100), launchForce);
        rBody.transform.rotation = Quaternion.Euler(0, 0, angle);
        rBody.AddForce(launchForce);
        this.parentAstronaut = parentAstronaut;
        camTarget = this.GetComponent<Transform>();
        projectileExistiert = true;
    }

    private void DespawnThisProjectile()
    {
        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        Astronaut parentAstronautScript = parentAstronaut.GetComponent<Astronaut>();
        parentAstronautScript.EndShootingPhase();
        Destroy(this.gameObject);
        camTrans.position = startCameraPosition;
        mainCamera.orthographicSize = startCameraSize;
        projectileExistiert = false;
    }

    void Explode()
    {
        timerStarted = true;
        if (explosion)
        {
            Debug.Log("explosion");
            Vector3 explosionPos = transform.position;
            // Get nearest Objects
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
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
        
    }
    void MoveCamera()
    {
        Vector3 offset = new Vector3(0, 0, -10);
        Vector3 desiredPosition = camTarget.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(camTrans.position, desiredPosition, smoothSpeed);
        camTrans.position = smoothPosition;
        targetZoom = targetZoom + zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 0.05f, 0.1f);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}





