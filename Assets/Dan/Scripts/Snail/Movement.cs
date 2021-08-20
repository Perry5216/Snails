using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

    public LayerMask layerMask;
    [HideInInspector]
    public Snail snail;
    private CustomPhysics customPhysics;
    
    private float maxDistance = 2f;
    

    private Vector3 origin;
    private Vector3 direction;

    private float currentHitDistance;

    private void Awake()
    {
        snail = GetComponent<Snail>();
    }

    private void Start()
    {        
        customPhysics = GetComponent<CustomPhysics>();
    } 

    private void Update()
    {
        if (snail.isDead)
        {
            customPhysics.ChangeDirectionOfGravity(new Vector3(0f, 0f, -9.81f));
            return;
        }
            
        if (snail.isCharging)
            return;
   
            snail.anim.SetBool("isMove", false);
            //anim.SetBool("isHit", false);
            origin = transform.position - (transform.TransformDirection(Vector3.forward) * 0.1f);
        if (snail.isActive)
        {
            if (Input.GetKey(KeyCode.A))
            {
                snail.anim.SetBool("isMove", true);
                //origin = transform.position - (transform.TransformDirection(Vector3.forward) * 0.1f);
                //origin -= transform.TransformDirection(Vector3.left) * 1f;
                transform.Translate((Vector3.left) * Time.deltaTime * 2f);
                if (snail.sprite != null)
                {
                    //sprite.transform.eulerAngles = new Vector3(270f, -90f, -90f);
                    snail.sprite.transform.localEulerAngles = new Vector3(270f, -90f, -90f);
                }

            }
            else if (Input.GetKey(KeyCode.D))
            {
                snail.anim.SetBool("isMove", true);
                //origin = transform.position - (transform.TransformDirection(Vector3.forward) * 0.1f);
                //origin -= transform.TransformDirection(-Vector3.left) * 1f;
                transform.Translate((-Vector3.left) * Time.deltaTime * 2f);
                if (snail.sprite != null)
                {
                    // sprite.transform.eulerAngles = new Vector3(90f, -90f, -90f);
                    snail.sprite.transform.localEulerAngles = new Vector3(90f, -90f, -90f);
                }

            }
        }     

        direction = -transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            customPhysics.ChangeDirectionOfGravity(-hit.normal * 200f);
        }
        else
        {
            currentHitDistance = maxDistance;
            customPhysics.ResetForce();
            customPhysics.ChangeDirectionOfGravity(new Vector3(0f, 0f, -9.81f));
        }
    }      

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        
       
    }    
}
