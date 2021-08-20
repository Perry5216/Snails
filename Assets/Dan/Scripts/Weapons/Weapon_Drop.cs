using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Drop : Weapon {
    private bool lookLeft;
    private bool hasDropped = false;

    [Header("Drop Settings")]
    public GameObject droppedProjectilePrefab;
    public GameObject dropPoint;    
    public float despawnTime = 5f;
  
    private void Start()
    {
        dropPoint.transform.position = transform.forward + transform.parent.transform.position - (transform.right * 2f);
        lookLeft = true;
    }
    override protected void Update()
    {
        if (!Operable())
            return;
        if (Input.GetKey(KeyCode.LeftArrow) && !lookLeft)
        {
            dropPoint.transform.position = transform.forward + transform.parent.transform.position - (transform.right * 2f);
            lookLeft = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && lookLeft)
        {
            dropPoint.transform.position = transform.forward + transform.parent.transform.position + (transform.right * 2f);
            lookLeft = false;
        }
        if (hasDropped)
            return;
        if (Input.GetKey(KeyCode.DownArrow))
        {         
            IsFiring(true);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {          
            GameObject projectile = Instantiate(droppedProjectilePrefab, dropPoint.transform.position, dropPoint.transform.rotation);
            projectile.transform.parent = null;

            Projectile_Walking projectile_Walking = projectile.GetComponent<Projectile_Walking>();
            projectile_Walking.Drop(lookLeft,despawnTime, hitPointValue);
            hasDropped = true;
            StartCoroutine(despawnTimer());
        }
    }
    IEnumerator despawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        IsFiring(false);
    }
}
