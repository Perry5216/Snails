using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Earthquake : Weapon {
    private Vector3 randomGravity;
    private bool hasChangedGravity = false;

    override protected void Update()
    {
        if (!Operable())
            return;
        if (Input.GetKey(KeyCode.DownArrow))
        {
            IsFiring(true);
            randomGravity = new Vector3(Random.Range(-200, 200), 0f, Random.Range(-200, 200));
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (!hasChangedGravity)
                StartCoroutine(ChangeGrav());
        }
    }

    IEnumerator ChangeGrav()
    {
        hasChangedGravity = true;
        snail.anim.SetBool("isFlying", true);
        Vector3 normalGrav = Physics.gravity;
        StartCoroutine(ChangeDirection());
        yield return new WaitForSeconds(5f);
        hasChangedGravity =false;
        CustomPhysics customPhysics = snail.GetComponent<CustomPhysics>();
        customPhysics.ResetForce();
        customPhysics.ChangeDirectionOfGravity(randomGravity);
        IsFiring(false);
        snail.anim.SetBool("isFlying", false);
    }

    IEnumerator ChangeDirection()
    {
        while (hasChangedGravity)
        {
            randomGravity = new Vector3(Random.Range(-200, 200), 0f, Random.Range(-200, 200));
            CustomPhysics customPhysics = snail.GetComponent<CustomPhysics>();
            customPhysics.ResetForce();
            customPhysics.ChangeDirectionOfGravity(randomGravity);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
