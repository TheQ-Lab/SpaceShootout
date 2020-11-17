using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    float delay = 3f;
    float countdown;

    bool hasExploded = false;

    //public GameObject explosionEffect;
    public float radius = 5f;
    public float force = 700f;

    void Start()
    {
        //countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        /*countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }*/
    }

    void Explode()
    {
        //Show effect
        //Instantiate(explosionEffect, transform.position, transform.rotation);
        // Get nearest Object
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(0, radius));
        //Add force
        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
               
            }

        }
        
        //Remove a bomb
        //Destroy(gameObject);
    }


}
