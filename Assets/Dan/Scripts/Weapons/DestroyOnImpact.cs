using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : Projectile
{
    [Header("Explosion Effect Settings")]
    public GameObject explosion1;
    public int radius = 2;
    [Header("Destroyer Settings")]
    public bool destroyTerrain = true;
    public bool ignoreDestroyers = true;


    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreDestroyers)
        {
            if (collision.gameObject.tag != "Destroyer")
                Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void OnDestroy()
    {
        if(explosion1 != null)
        {
            GameObject currentExplosion = Instantiate(explosion1, transform.position, Quaternion.identity);
            currentExplosion.transform.localScale = new Vector3(radius, radius, radius);
            Destroy(currentExplosion, 0.5f);
        }

        GameObject[] snails = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject snail in snails)
        {
            float distance = Vector3.Distance(transform.position, snail.transform.position);
            if(distance <= radius)
            {
                Snail snailProperties = snail.GetComponent<Snail>();
                snailProperties.health.DecreaseHealth(damageValue);                         
            }
        }
    }
   
}
