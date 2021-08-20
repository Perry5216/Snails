using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wepon : MonoBehaviour
{

    protected bool endflag = false;
    public Sprite icon; // conatians image that is displayed on the UI for the wepon

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        weponfin(endflag);
    }
    protected void weponfin(bool endflag)
    {
        if (endflag == true)
        {
            Gamecontroler contorler = GameObject.Find("GameController").GetComponent(typeof(Gamecontroler)) as Gamecontroler;
            contorler.turne_flag = true;
            Destroy(gameObject);
        }
    }
}
