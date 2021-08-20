using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wepon_button : MonoBehaviour
{
    public int button_num;
    public GameObject this_wepon;
    public snails snail;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Gamecontroler GC = Gamecontroler.gm;// GameObject.Find("GameController").GetComponent(typeof(Gamecontroler)) as Gamecontroler; //conets the UI button to the wepon in the invetory of the snail
        // snail = GC.current_snail.GetComponent(typeof(snails)) as snails;
        // this_wepon = snail.invetory[button_num];

        Snail snail;
        if (Gamecontroler.gm.current_snail != null)
        {
            snail = Gamecontroler.gm.current_snail.GetComponent<Snail>();
            if (snail != null)
            {
                

                GetComponent<Button>().GetComponent<Image>().sprite = snail.weapons[button_num].GetComponent<Weapon>().icon;

            }
        }
             

     


        //GetComponent<Button>().GetComponent<Image>().sprite = this_wepon.GetComponent<wepon>().icon;
    }


}
