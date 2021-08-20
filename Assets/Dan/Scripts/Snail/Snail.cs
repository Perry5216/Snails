using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
public class Snail : MonoBehaviour {
    [Header("Is This Snails Turn")]
    public bool isActive = false;
    public bool hasShot = false;

    [HideInInspector]
    public Movement movement;
    [HideInInspector]
    public Health health;
    [HideInInspector]
    public AudioSource aSource;

    [Header("Snail Settings")]
    public bool destroyIfOutOfBounds = true;
    public Transform statusEffectPosition;
    private bool statusEffectActive = false;
    [HideInInspector]
    public int teamID = 0;
    [Space]
    public int soundID = -1;
    [Space]

    [Header("Animation")]
    public GameObject sprite;
    public Animator anim;

    [Header("Health")]
    public float maxHealth = 500f;
    public Slider healthBar;
    public GameObject canvas;
    [HideInInspector]
    public bool isDead = false;



    [Header("Weapon Settings")]
    public GameObject[] weapons;
    [HideInInspector]
    public GameObject currentWeapon;
    [HideInInspector]
    private int currentWeaponIndex;
    public bool isCharging = false;
    public Text power;    

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        aSource = GetComponent<AudioSource>();
        weapons = new GameObject[3];
        if (soundID == -1)
            soundID = Random.Range(0, 4);
    }

    void Start () {
        //if (weapons.Length > 0)
        //{
        //    currentWeapon = Instantiate(weapons[0], transform);
        //    currentWeapon.transform.parent = gameObject.transform;
        //}
	}
	
    public void Initialise(int _teamID)
    {
        teamID = _teamID;
        //CHANGE COLOUR OF SNAIL HERE!
        Debug.Log("MY ID IS : " + teamID);
        if (teamID == 1)
            anim.SetBool("isGreen", true);
        else
            anim.SetBool("isPurple", true);

        for (int i = 0; i < 3; i++)
        {
            int selectedWeapon = Random.Range(0, Gamecontroler.gm.weapons.Length);
            weapons[i] = Gamecontroler.gm.weapons[selectedWeapon];
        }
        SwitchWeapon(0);    
    }

	
	//void Update () {
 //       if (isCharging)
 //           return;
 //       if (!isActive)
 //           return;
 //       if (Input.GetKeyUp(KeyCode.Alpha1))
 //       {
 //           SwitchWeapon(0);
 //       }
 //       if (Input.GetKeyUp(KeyCode.Alpha2))
 //       {
 //           SwitchWeapon(1);
 //       }
 //       if (Input.GetKeyUp(KeyCode.Alpha3))
 //       {
 //           SwitchWeapon(2);
 //       }
 //       if (Input.GetKeyUp(KeyCode.Alpha4))
 //       {
 //           SwitchWeapon(3);
 //       }
 //       if (Input.GetKeyUp(KeyCode.Alpha5))
 //       {
 //           SwitchWeapon(4);
 //       }
 //       if (Input.GetKeyUp(KeyCode.Alpha6))
 //       {
 //           SwitchWeapon(5);
 //       }
 //       if (Input.GetKeyUp(KeyCode.Alpha7))
 //       {
 //           SwitchWeapon(6);
 //       }
 //   }

    public void SwitchWeapon(int index)
    {
        if (isCharging)
            return;
        if (index >= weapons.Length)
            return;
        if(weapons.Length > 0)
        {
            Destroy(currentWeapon);
            currentWeapon = Instantiate(weapons[index], transform);
            currentWeapon.transform.parent = gameObject.transform;
            currentWeaponIndex = index;
        }
        power.text = "";
    }

    public void SwitchWeaponSlot()
    {
        Destroy(currentWeapon);
        int selectedWeapon = Random.Range(0, Gamecontroler.gm.weapons.Length - 1);
        weapons[currentWeaponIndex] = Gamecontroler.gm.weapons[selectedWeapon];
    }

    public void PlaySoundEffect(bool ignoreDeath = false)
    {
        if(!ignoreDeath)
            if (isDead)
                return;
        if (aSource == null)
            return;
        if (!aSource.isPlaying)
        {
           aSource.pitch = Random.Range(1, 3);            
           aSource.clip = Sound_Manager.sm.GetHitSound(soundID);         
           aSource.Play();
        }   
    }

    public void AddStatusEffect(GameObject statusEffect)
    {
        if(!statusEffectActive && !isDead)
            StartCoroutine(StatusEffect(statusEffect));
    }

    private IEnumerator StatusEffect(GameObject statusEffect)
    {
        statusEffectActive = true;        
        if (statusEffectPosition != null)
        {
            GameObject status = Instantiate(statusEffect, statusEffectPosition.transform);
            Destroy(status, 2f);
        }
        else
        {
            Debug.Log("No Status Effect Transform Assigned");
        }
        yield return new WaitForSeconds(2f);
        statusEffectActive = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            PlaySoundEffect(true);
            isDead = true;            
            gameObject.tag = "Dead";
            gameObject.SetActive(false);
        }
    }
}
