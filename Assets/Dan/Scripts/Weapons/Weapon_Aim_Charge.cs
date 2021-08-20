using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Aim_Charge : Weapon_Aim {  
    private float currentFirePower;

    [Header("Power Settings")]
    public float maxFirePower = 25f;  
    public float firePowerRate = 5f;
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public bool applySpin = true;   

    override protected void Update()
    {
        if (!Operable())
            return;
        base.Update();     
        if (projectilePrefab != null)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (aSource != null && soundFx != null)
                    if (!aSource.isPlaying)
                        aSource.Play();
                currentFirePower += firePowerRate * Time.deltaTime;
                if (currentFirePower > maxFirePower)
                    currentFirePower = maxFirePower;
                int percentPower = Mathf.CeilToInt((currentFirePower / maxFirePower) * 100) ;
                IsFiring(true, percentPower);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (aSource != null && soundFx != null)
                    aSource.Stop();
                GameObject projectile = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
                projectile.transform.parent = null;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                if(projectileScript != null)
                {
                    float percentageFirePower = currentFirePower / maxFirePower;
                    projectileScript.Properties(hitPointValue, percentageFirePower);
                }
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = firePoint.transform.forward * currentFirePower;
                if (applySpin)
                    rb.AddTorque(new Vector3(0f, 2f, 0f));
                Destroy(projectile, 5.5f);
                currentFirePower = 0;
                IsFiring(false);
            }
        }
    }
}
