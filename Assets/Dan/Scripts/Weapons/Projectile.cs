using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {
    protected float damageValue;
    public string playerTag = "Snail";
    [Header("Sound Settings")]
    public AudioClip soundEffect;

    public void Properties(float _damageValue, float chargePercentage = 0, AudioClip soundFx = null)
    {
        if (soundFx != null)
            soundEffect = soundFx;
        if (chargePercentage == 0)
            damageValue = _damageValue;
        else
            damageValue = _damageValue * chargePercentage;
    }
	
}
