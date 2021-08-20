using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Split : Projectile
{
    private Rigidbody rb;

    [Header("Explosion Effect Settings")]
    public GameObject explosion1;
    public int radius = 2;   
    [Header("Split Settings")]
    public GameObject splitPrefabs;    
    public int splitAmount = 3;
    public float lifeTime = 2f;    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != "Destroyer")
                Destroy(gameObject);
       
    }
    private void OnDestroy()
    {
        if (explosion1 != null)
        {
            GameObject currentExplosion = Instantiate(explosion1, transform.position, Quaternion.identity);
            currentExplosion.transform.localScale = new Vector3(radius, radius, radius);
            Destroy(currentExplosion, 0.5f);
        }

        GameObject[] snails = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject snail in snails)
        {
            float distance = Vector3.Distance(transform.position, snail.transform.position);
            if (distance <= radius)
            {
                Snail snailProperties = snail.GetComponent<Snail>();
                snailProperties.health.DecreaseHealth(damageValue);
            }
        }

        for (int i = 0; i < splitAmount; i++)
        {
            GameObject splitPrefab = Instantiate(splitPrefabs, transform.position, transform.rotation);
            splitPrefab.transform.parent = null;
            splitPrefab.GetComponent<Rigidbody>().velocity = (rb.velocity);

            DestroyOnImpact impactScript = splitPrefab.GetComponent<DestroyOnImpact>();
            if (impactScript != null)
            {
                impactScript.Properties(damageValue, 1);
            }
        }
    }
}
