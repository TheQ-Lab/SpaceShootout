using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{

    void Start()
    {
        projectileType = "missile";

        // damage
        damage = 50;
        radius = 0.8f;
        

        base.rBody = GetComponent<Rigidbody2D>();
        FindObjectOfType<AudioManager>().Play("missileFly");
    }

    IEnumerator Explode()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D nearbyObject in colliders)
        {
            //Debug.Log(nearbyObject);
            if (nearbyObject.GetComponent<Astronaut>())
            {
                Vector3 offset = new Vector3(0, 0, -5);
                Vector3 explosionPos = transform.position + offset;
                Instantiate(explosionEffect, explosionPos, Quaternion.LookRotation(base.rBody.velocity));
                FindObjectOfType<AudioManager>().Play("missileExplosion");
                Debug.Log("Do something, cause collider is with astronaut");
                Astronaut victim = nearbyObject.GetComponent<Astronaut>();
                victim.Damage(damage);
                break;
            }
        }
        yield return DespawnThisProjectile();
    }


    void Update()
    {
        if (base.rBody.velocity!=Vector2.zero)
        {
            Quaternion dir = Quaternion.LookRotation(base.rBody.velocity);
            dir *= Quaternion.Euler(0, 0, -90f);
            base.rBody.SetRotation(dir);
        }
    }

}
