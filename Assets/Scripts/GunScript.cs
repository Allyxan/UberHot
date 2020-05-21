using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public static GunScript instance;
    public Animator anim;
    public static bool throww = false;
    public static bool shoot = false;
    public static bool pickup = false;
    public static bool open = false;
    public static bool doing = true;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("state", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.parent)
        {
            if ((this.transform.parent.parent.name) == "WeaponPlace")
            {
                anim = GetComponent<Animator>();
                anim.SetBool("state", true);
 
                if (HandScript.doing == true)
                {
                    if (throww == true)
                    {
                        HandScript.instance.anim.SetTrigger("throw");
                        anim.SetTrigger("throw");
                        throww = false;
                    }
                    if (shoot == true)
                    {
                        HandScript.instance.anim.SetTrigger("Shooting");
                        anim.SetTrigger("Shooting");
                        shoot = false;
                    }
                    if (pickup == true)
                    {
                        anim.SetBool("state", true);
                        anim.SetTrigger("pick up");
                        HandScript.instance.anim.SetTrigger("pick up");
                        pickup = false;
                    }
                    if (open == true)
                    {
                        anim.SetTrigger("open");
                        open = false;
                    }
                }
            }
            else
            {
                if (anim != null)
                    anim.SetBool("state", false);
                anim = null;
            }
        }
        else
        {
            if (anim != null)
                anim.SetBool("state", false);
            anim = null;
        }
    }
}
