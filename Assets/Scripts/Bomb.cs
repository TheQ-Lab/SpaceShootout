using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{

    void Start()
    {
        projectileType = "bomb";

        // damage
        damage = 50;
        delay = 3f;
        radius = 5f;
    }
    protected new void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.name == "ProjectileBarrier")
        {
            delay = 0f;
        }
        base.OnCollisionEnter2D(collision);
    }


    IEnumerator Explode()
    {
       // Vector3 offsetBombPos = new Vector3 (0, 0, 0.35f);
       // rBody.transform.position = transform.position + offsetBombPos;
        Debug.Log(rBody.transform.position);
        rBody.drag = 5;

        Debug.Log("explosion");
        yield return new WaitForSeconds(delay);
        Vector3 offset = new Vector3(0, 0, -10);
        Vector3 explosionPos = transform.position + offset;
        Instantiate(explosionEffect, explosionPos, Quaternion.LookRotation(rBody.velocity));

        // Get nearest Objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D nearbyObject in colliders)
        {
            //Debug.Log(nearbyObject);
            if (nearbyObject.GetComponent<Astronaut>())
            {
                Debug.Log("Do something, cause collider is with astronaut");
                Astronaut victim = nearbyObject.GetComponent<Astronaut>();
                victim.Damage(damage);
                
            }
        }
        yield return DespawnThisProjectile();
    }

    void Update()
    {
       
    }

 


}
