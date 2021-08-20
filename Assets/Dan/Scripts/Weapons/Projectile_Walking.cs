using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Walking : Projectile {
    private CustomPhysics customPhysics;
    private float maxDistance = 2f;
    private Vector3 origin;
    private Vector3 direction;
    private float currentHitDistance;
    private bool left;

    [Header("Walk Settings")]
    public LayerMask layerMask;
    [Header("Sprite Settings")]
    public GameObject sprite;    
    [Header("Status Effect Settings")]
    public GameObject statusEffect;
    [Header("Other Sound Settings")]
    public bool pauseOtherSounds = false;
    public float[] times;

    private void Start()
    {
        customPhysics = GetComponent<CustomPhysics>();
    }

    public void Drop(bool facingLeft, float despawnTimer, float damage)
    {
        left = facingLeft;
        damageValue = damage;
        if(soundEffect!=null)
        {
            AudioSource aSource = GetComponent<AudioSource>();
            if(aSource != null)
            {
                if (pauseOtherSounds)
                    Sound_Manager.sm.PauseUnPauseMusic(true);
                aSource.clip = soundEffect;
                if (times.Length != 0)         
                    aSource.time = times[Random.Range(0, times.Length)];
                aSource.Play();
            }
        }
        Destroy(gameObject, despawnTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Snail snail = collision.gameObject.GetComponent<Snail>();
        if (snail != null)
        {
            snail.health.DecreaseHealth(damageValue);
            if (statusEffect != null)
                    snail.AddStatusEffect(statusEffect);
        }
    }

    void Update()
    {
        if (left)
        {
            transform.Translate((Vector3.left) * Time.deltaTime * 2f);
            if (sprite != null)
                sprite.transform.localEulerAngles = new Vector3(270f, -90f, -90f);
        }
        else
        {
            transform.Translate((-Vector3.left) * Time.deltaTime * 2f);
            if (sprite != null)        
                sprite.transform.localEulerAngles = new Vector3(90f, -90f, -90f);
        }
     

        origin = transform.position - (transform.TransformDirection(Vector3.forward) * 0.1f);
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
    private void OnDestroy()
    {
        if (soundEffect != null)
            if (pauseOtherSounds)
                Sound_Manager.sm.PauseUnPauseMusic(false);
    }
}
