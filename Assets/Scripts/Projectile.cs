using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType = "";
    public int WeaponDamage = 100;

    private Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(projectileType == "Missile")
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Astronaut astronautScript = collision.gameObject.GetComponent<Astronaut>();
        if (astronautScript != null)
        {
            Debug.Log("Y ooooo, thats an Astronaut, letzs go!");
            astronautScript.Damage(WeaponDamage);
        }

        DespawnThisProjectile();
    }

    public void SetInitialParameters(string _projectileType, Vector2 launchForce)
    {
        projectileType = _projectileType;
        rBody.AddForce(launchForce);

        float angle = Vector2.SignedAngle(new Vector2(0, 100), launchForce);
        rBody.transform.rotation = Quaternion.Euler(0, 0, angle);

        rBody.AddForce(launchForce);
    }

    private void DespawnThisProjectile()
    {
        GravityManager.Instance.RemoveAnyObjectFromGravity(this.gameObject);
        Destroy(this.gameObject);
    }
}
