using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wepon_buton_Behaviour : MonoBehaviour
{

    public void TaskOnClick(int button_num)
    {

        // GameObject wepon_ref = GetComponent<Button>().GetComponent<wepon_button>().this_wepon; // spwarns wepon prefab on current snail
        //wepon_ref.SetActive(true);

        //        Debug.Log(wepon_ref);

        //        Destroy(wepon_ref);

        Snail snail = Gamecontroler.gm.current_snail.GetComponent<Snail>();

        snail.SwitchWeapon(button_num);
    }
}