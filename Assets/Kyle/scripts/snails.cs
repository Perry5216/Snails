using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snails : MonoBehaviour
{
    int health = 10;
    int inventory_slot = 0;
    public GameObject[] invetory = new GameObject[3];


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (invetory[inventory_slot] == null) // used to repopulate the snails inventory with wepons
        {
            Debug.Log("new wepon");
            //string wepon_number = Convert.ToString(UnityEngine.Random.Range(0, 2));
            //string wepon_spwarn = "wepon" + wepon_number;
            //GameObject wepon_obj = Instantiate(Resources.Load(wepon_spwarn) as GameObject, this.transform);
            //invetory[inventory_slot] = wepon_obj;

            //Select Random Weapon
            int selectedWeapon = UnityEngine.Random.Range(0, Gamecontroler.gm.weapons.Length - 1);
            //Give Ranomd Weapon
            GameObject weapon_obj = Instantiate(Gamecontroler.gm.weapons[selectedWeapon], transform);
            //Give to inventory
            invetory[inventory_slot] = weapon_obj;


        }

        inventory_slot++; //used to itorate therw the snails iventory
        if (inventory_slot > 2)
        {
            inventory_slot = 0;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}