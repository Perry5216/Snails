using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtOnImpact : Projectile
{
    [Header("Impact Settings")]
    public bool destroyOnImpact = true;
    public bool isTrigger = false;
    [Header("Status Effect Settings")]
    public GameObject statusEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTrigger)
        {
            Snail snail = collision.gameObject.GetComponent<Snail>();
            if(snail != null)
            {
                snail.health.DecreaseHealth(damageValue);
                if (statusEffect != null)
                    snail.AddStatusEffect(statusEffect);
            }           
            if (destroyOnImpact)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger)
        {
            Snail snail = other.gameObject.GetComponent<Snail>();
            if (snail != null)
            {
                snail.health.DecreaseHealth(damageValue);
                if (statusEffect != null)
                    snail.AddStatusEffect(statusEffect);
            }
            if (destroyOnImpact)
                Destroy(gameObject);
        }
    }

}
