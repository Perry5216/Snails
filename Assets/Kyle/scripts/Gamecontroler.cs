using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gamecontroler : MonoBehaviour
{
    public GameObject[] weapons;


    public static Gamecontroler gm;

    public GameObject snail;


    public int nextSnail; // these variblaes are used to determin the next snaile in the next turn
    public int nextTeam;

    //THESE VALUES WERE 3
    const int number_of_teams = 2; // these are used to define an array that contains all the snailes
    const int number_of_snails = 4;
    GameObject[,] Teams = new GameObject[number_of_teams, number_of_snails];

    const int max_snails = number_of_teams * number_of_snails; // this is used to enshure that the max number of snails is not exseaed
    int snaile_number = 0;

    int game_phase = 1; // tells the system what stage of the game we are at 

    public bool turne_flag = true; //used to controll the the end of each turn
    public GameObject current_snail;

    private Vector3 mousepos;

    // Use this for initialization

    private void Awake()
    {
        gm = this;
    }

    void Start()
    {
        nextSnail = 0;
        nextTeam = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mousepos = Input.mousePosition;
        mousepos = new Vector3(mousepos.x, mousepos.y ,mousepos.z);
        mousepos = Camera.main.ScreenToWorldPoint(mousepos); // used to get the mouse position realtive to the screen

        if (snaile_number < max_snails && game_phase == 1)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                string team_number;
                string snail_spwarn;

                snaile_number++;



                team_number = Convert.ToString(nextTeam + 1); // this deatermins what snail we are spwarning
                snail_spwarn = "snail team" + team_number;

                Debug.Log(snail_spwarn);

                Vector3 newPosition = new Vector3(mousepos.x, -0.5f, mousepos.z);


              //  GameObject snail_obj = Resources.Load(snail_spwarn) as GameObject;
                GameObject snail_chater = Instantiate(snail, newPosition, Quaternion.identity);
                Snail snailScript = snail_chater.GetComponent<Snail>();
                snailScript.Initialise(nextTeam + 1);
                Teams[nextTeam, nextSnail] = snail_chater;

                nextTeam++; // this is used to iterate therew the array
                if (nextTeam == number_of_teams)
                {
                    nextSnail++;
                    nextTeam = 0;
                }
                if (nextSnail == number_of_snails)
                {
                    nextSnail = 0;
                }
                if (snaile_number == max_snails)
                {
                    game_phase = 2;
                }

            }
        }

        if (game_phase == 2)
        {

            current_snail = Teams[nextTeam, nextSnail];

            Snail snailScript = current_snail.GetComponent<Snail>();
            snailScript.isActive = true;

            //current_snail.movementscript bool varible  = true
            if (snailScript.isDead)
            {
                turne_flag = true;
            }


            if (turne_flag == true)
            {
                turne_flag = false;
                snailScript.isActive = false;

                GameObject[] livingSnails = GameObject.FindGameObjectsWithTag("Snail");

                bool isDifferent = false;

                foreach (GameObject item in livingSnails)
                {
                    Snail tempSnail = item.GetComponent<Snail>();
                    if (tempSnail.teamID != current_snail.GetComponent<Snail>().teamID)
                        isDifferent = true;
                }

                if (!isDifferent)
                {
                    game_phase = 3;
                }

                //CAMERA THING HERE!
                //Camera.main.transform.position = new Vector3(Teams[nextTeam, nextSnail].transform.position.x, Camera.main.transform.position.y, Teams[nextTeam, nextSnail].transform.position.z);

                nextTeam++;
                if (nextTeam == number_of_teams)
                {
                    nextSnail++;
                    nextTeam = 0;
                }
                if (nextSnail == number_of_snails)
                {
                    nextSnail = 0;
                }
            }

        }

        if (game_phase == 3)
        {
            Debug.Log(current_snail + "'s team has won! wooo!");
        }

    }
}
