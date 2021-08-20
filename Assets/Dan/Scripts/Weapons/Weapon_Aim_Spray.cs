using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Aim_Spray : Weapon_Aim {
    private bool spraying = false;

    [Header("Projectile Settings")]
    public GameObject sprayPrefab;
    public int moleculeAmount = 300;
    public float sprayInterval = 0.01f;
    public float despawnTime = 5.5f;
    public float firePower = 15f;

    override protected void Update()
    {
        if (!Operable())
            return;
        base.Update();
        if (sprayPrefab != null)
        {
            if (spraying)
                return;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                IsFiring(true);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (spraying)
                    return;
                StartCoroutine(Spray());
            }
        }
    }

    IEnumerator Spray()
    {
        spraying = true;
        for (int i = 0; i < moleculeAmount; i++)
        {
            GameObject projectile = Instantiate(sprayPrefab, firePoint.transform.position, firePoint.transform.rotation);
            projectile.transform.parent = null;
            HurtOnImpact hurtOnImpact = projectile.GetComponent<HurtOnImpact>();
            hurtOnImpact.Properties(hitPointValue);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.transform.forward * firePower;           
            rb.AddTorque(new Vector3(0f, 2f, 0f));
            Destroy(projectile, despawnTime);
            yield return new WaitForSeconds(sprayInterval);
        }
        IsFiring(false);
        spraying = false;
    }

}
