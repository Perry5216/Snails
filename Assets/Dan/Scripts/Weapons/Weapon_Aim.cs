using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Aim : Weapon {
    protected Quaternion startRotation;

    [Header("Arm Settings")]
    public GameObject arm;
    public GameObject firePoint;
    
    override protected void Awake()
    {
        snail = gameObject.transform.parent.GetComponent<Snail>();
        startRotation = transform.rotation;
    }

    protected void LateUpdate()
    {
        transform.rotation = startRotation;
    }

    override protected void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            arm.transform.Rotate(new Vector3(0, -1, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            arm.transform.Rotate(new Vector3(0, 1, 0));
        }
    }
    
}
