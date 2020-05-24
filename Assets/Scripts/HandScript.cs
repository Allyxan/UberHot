using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;
    public Animator anim;
    public static bool open = false;
    public static bool bb1 = false;
    public static bool idle = false;
    int IdleNoGun;
    int IdleGun;
    int IdleBBGun;
    public static bool doing = false;
    public static bool bb = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        anim = GetComponent<Animator>();
        anim.SetBool("state", true);
        IdleNoGun = Animator.StringToHash("Base Layer.idle without gun");
        IdleGun = Animator.StringToHash("Base Layer.idle with gun");
        IdleBBGun = Animator.StringToHash("Base Layer.idle bb gun");
    }

    // Update is called once per frame
    void Update()
    {
        if ((anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleNoGun) || (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleGun) || (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleBBGun))
        {
            doing = true;
        }
        else
        {
            doing = false;
        }

        if (bb == true)
        {
            anim.SetBool("bb", true);
        }
        else
        {
            anim.SetBool("bb", false);
        }
        //Debug.Log(doing);
        if (doing == true)
        {
            if (open == true)
            {
                anim.SetTrigger("open");
                //  HandScript.instance.anim.SetTrigger("open");
                open = false;
            }
            if (bb1 == true)
            {
                anim.SetTrigger("bb1");
                // HandScript.instance.anim.SetTrigger("open");
                bb1 = false;
            }
        }
    }
}
