using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_First_Aid : Weapon {
    [Header("Health Status Effect Settings")]
    public GameObject statusEffect; 
    
    override protected void Update () {
        if (!Operable())
            return;
        if (Input.GetKey(KeyCode.DownArrow))
        {
            snail.isCharging = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            IsFiring(false);
            snail.AddStatusEffect(statusEffect);
            snail.health.IncreaseHealth(hitPointValue);
        }
    }
}
