using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [HideInInspector]
    public Snail snail;

    private float currentHealth;
    
    private void Awake()
    {
        snail = GetComponent<Snail>();
    }
    void Start () {
        currentHealth = snail.maxHealth;
        if (snail.healthBar != null)
        {
            snail.healthBar.maxValue = snail.maxHealth;
        }
        UpdateBar();
    }
    
    public void DecreaseHealth(float value)
    {
        snail.PlaySoundEffect();
        currentHealth -= value;
        snail.anim.SetTrigger("isHit");
        UpdateBar();
        Death();
    }

    public void IncreaseHealth(float value)
    {
        currentHealth += value;
        if(currentHealth > snail.maxHealth)
        {
            currentHealth = snail.maxHealth;
        }
        UpdateBar();
    }
    private void Death()
    {
        if(currentHealth <= 0)
        {
            snail.anim.SetBool("isDead", true);
            snail.isDead = true;
            snail.gameObject.tag = "Dead";
            if(snail.canvas != null)
                snail.canvas.active = false;
            if(snail.currentWeapon != null)
                snail.currentWeapon.active = false;
        }
    }

    private void UpdateBar()
    {
        if(snail.healthBar != null)
        {
            snail.healthBar.value = currentHealth;
        }
        else
        {
            Debug.Log(gameObject + " is missing a health bar.");
        }
    }
}
