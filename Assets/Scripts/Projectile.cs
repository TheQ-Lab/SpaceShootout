using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Type {Missile, Bomb};

    public string projectileType; // name
    public float lifetime;

    protected Rigidbody2D rBody;

    private float deathTimer;
    private GameObject parentAstronaut;

    public GameObject explosionEffect;
    public float explosionEffectDuration;

    // damage
    protected int damage = 50;
    protected float delay;
    protected float radius;

   

    //for camera movement
    Transform camTarget;
    Transform camTrans;
    private Camera mainCamera;
    private Vector3 startCameraPosition;
    private float smoothSpeed = 0.125f;
    private Animator cameraAnimator;

    private bool explosionStarted = false;
    private bool isProjectileExistent = false;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Time.time >= deathTimer + lifetime) {
            delay = 0;
            if (!explosionStarted)
            {
                explosionStarted = true;
                StartCoroutine("Explode");
            }
        }

        if (isProjectileExistent)
        {
            MoveCamera();
        }
    }

    IEnumerator Explode()
    {

        // implement in real class
        yield return null;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (!explosionStarted)
        {
            explosionStarted = true;
            StartCoroutine("Explode");
        }
    }

    public void launch(Vector2 launchForce, float lifetime, GameObject parentAstronaut)
    {
        deathTimer = Time.time;
        mainCamera = Camera.main;
        startCameraPosition = mainCamera.transform.position;
        camTrans = mainCamera.GetComponent<Transform>();
        cameraAnimator = mainCamera.GetComponent<Animator>();
        //
        this.lifetime = lifetime;
        float angle = Vector2.SignedAngle(new Vector2(0, 100), launchForce);
        rBody.transform.rotation = Quaternion.Euler(0, 0, angle);
        rBody.AddForce(launchForce);
        this.parentAstronaut = parentAstronaut;
        camTarget = this.GetComponent<Transform>();
        isProjectileExistent = true;
    }

    protected IEnumerator DespawnThisProjectile()
    {
        isProjectileExistent = false;
        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        this.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(explosionEffectDuration);
        //             
        camTrans.position = startCameraPosition;
        cameraAnimator.SetBool("Zoomed In", false);
        //
        Astronaut parentAstronautScript = parentAstronaut.GetComponent<Astronaut>();
        parentAstronautScript.EndShootingPhase();

        Destroy(this.gameObject);
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





