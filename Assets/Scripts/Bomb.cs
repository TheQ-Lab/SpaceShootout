using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : Projectile
{
   
    public float countDown;
    private Vector3 plusSize = new Vector3(0.05f, 0.05f, 0.05f);

    void Start()
    {
        projectileType = "bomb";

        // damage
        damage = 50;
        delay = 1.5f;
        radius = 7f;
    }
    protected new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name.Contains("Planet"))
        {
            rBody.drag = 15f;
        }
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
        

        Debug.Log("explosion");
        yield return Timer(delay);
        Vector3 offset = new Vector3(0, 0, -10);
        Vector3 explosionPos = transform.position + offset;
        Instantiate(explosionEffect, explosionPos, Quaternion.LookRotation(rBody.velocity));
        FindObjectOfType<AudioManager>().Play("bombExplosion");

        

        // Get nearest Objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D nearbyObject in colliders)
        {
            //Debug.Log(nearbyObject);
            if (nearbyObject.GetComponent<Astronaut>())
            {
                Debug.Log("Do something, cause collider is with astronaut");
                Astronaut victim = nearbyObject.GetComponent<Astronaut>();
                float dist = Vector3.Distance(victim.transform.position, transform.position);
                Debug.Log("DIST " + dist);
                int dmg;
                if (dist > radius)
                {
                    dmg = 0;
                }
                else
                {
                    dmg = (int)Mathf.Lerp(damage, 0, dist / radius);
                }
                Debug.Log("DMG " + dmg);
                victim.Damage(dmg);
                
            }
        }
        yield return DespawnThisProjectile();
    }

    IEnumerator Timer(float delay)
    {
        
        for (countDown = delay; countDown > 0; countDown -=0.1f)
        {
            rBody.GetComponent<MeshRenderer>().material.color = Color.red;
            rBody.transform.localScale += plusSize;
            FindObjectOfType<AudioManager>().Play("bombWait");

            //Debug.Log(rBody.transform.localScale);

            if (rBody.GetComponent<MeshRenderer>().enabled)
            {
                rBody.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                rBody.GetComponent<MeshRenderer>().enabled = true;
            }
            
            
            yield return new WaitForSeconds(0.1f);
        }
    }



    void Update()
    {
       
    }
   

 


}
