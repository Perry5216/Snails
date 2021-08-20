using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Aim_Dig : Weapon_Aim {
    [Header("Dig Settings")]
    public LayerMask layerMask;
    public float digDistance = 2f;
    public float digDuration = 10f;

    void Start () {
        firePoint.SetActive(false);
    }

    override protected void Update()
    {
        if (!Operable())
            return;
        base.Update();      
        if (Input.GetKey(KeyCode.DownArrow))
        {
            firePoint.SetActive(true);
            digDuration -= 0.5f * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            firePoint.SetActive(false);
        }
        if(digDuration <= 0)
        {
            firePoint.SetActive(false);
            IsFiring(false);
        }
    }
}
