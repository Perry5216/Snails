using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyOnImpact))]
public class Weapon : MonoBehaviour
{
    [HideInInspector]
    protected Snail snail;

    [Header("Icon Settings")]
    public Sprite icon;
    [Header("Damage/Health Value")]
    public float hitPointValue;
    [Header("Sound Effects")]
    public AudioClip soundFx;
    protected AudioSource aSource;

    virtual protected void Awake()
    {
        snail = gameObject.transform.parent.GetComponent<Snail>();
    }

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        if (aSource != null && soundFx != null)
            aSource.clip = soundFx;
    }

    virtual protected void Update()
    {
        Debug.Log("Weapon Needs a Firing Mechanism!");
    }

    protected void IsFiring(bool active, int percentagePower = 0)
    {
        if(percentagePower == 0)
            snail.power.text = "";
        else
            snail.power.text = percentagePower + "%";
        snail.isCharging = active;
        snail.anim.SetBool("isFiring", active);
        if(active == false)
        {
            Gamecontroler.gm.turne_flag = true;
            snail.SwitchWeaponSlot();
        }
    }

    protected bool Operable()
    {
        if (snail.isDead)
            return false;
        if (!snail.isActive)
            return false;
        return true;
    }
}
