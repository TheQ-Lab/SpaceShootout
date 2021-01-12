using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifetime;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyExplosion());
        
    }

    IEnumerator destroyExplosion()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
