using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    public bool useGravity = true;
    public Rigidbody rigidbody;
    public Vector3 direction = new Vector3(0f, 0f, -9.81f);
    public void Start()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        //Physics.gravity = direction;        
    }

    public void Update()
    {
        //Physics.gravity = direction;
        if(useGravity)
            rigidbody.velocity += direction * Time.deltaTime;
    }      
    
    public void ResetForce()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void ChangeDirectionOfGravity(Vector3 normal)
    {
            direction = normal;
    }


}
