using System.Collections;
using System.Collections.Generic;
using UnityEngine;










/// <summary>
/// DEPRECIATED
/// </summary>
public class AimCharge : MonoBehaviour {
    [HideInInspector]
    public Snail snail;
    private float currentFirePower;


    private void Awake()
    {
        snail = GetComponent<Snail>();
    }

    //void Update () {
    //    if (snail.isDead)
    //        return;
    //    //if (!snail.isCharging)
    //    //{
    //        if (Input.GetKey(KeyCode.LeftArrow))
    //        {
    //            snail.arm.transform.Rotate(new Vector3(0, -1, 0));
    //        }
    //        else if (Input.GetKey(KeyCode.RightArrow))
    //        {
    //            snail.arm.transform.Rotate(new Vector3(0, 1, 0));
    //        }
    //    //}
     

    //    if(snail.currentWeapon != null)
    //    {
    //        if (Input.GetKey(KeyCode.Space))
    //        {
    //            currentFirePower += snail.firePowerRate * Time.deltaTime;
    //            if (currentFirePower > snail.maxFirePower)
    //                currentFirePower = snail.maxFirePower;
    //            snail.isCharging = true;
    //            snail.anim.SetBool("isFiring", true);
    //        }

    //        if (Input.GetKeyUp(KeyCode.Space))
    //        {
    //            GameObject projectile = Instantiate(snail.currentWeapon, snail.firePoint.transform.position, snail.firePoint.transform.rotation);
    //            projectile.transform.parent = null;

    //            DestroyOnImpact impactScript = projectile.GetComponent<DestroyOnImpact>();
    //            if(impactScript != null)
    //            {
    //                float percentageFirePower = currentFirePower / snail.maxFirePower;
    //                impactScript.FireProperties(snail.damage, percentageFirePower);
    //            }               

    //            Rigidbody rb = projectile.GetComponent<Rigidbody>();
    //            rb.velocity = snail.firePoint.transform.forward * currentFirePower;
    //            if (snail.applySpin)
    //                rb.AddTorque(new Vector3(0f, 2f, 0f));
    //            Destroy(projectile, 5.5f);
    //            currentFirePower = 0;
    //            snail.isCharging = false;
    //            snail.anim.SetBool("isFiring", false);
    //        }
    //    }        
    //}
}
