using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;
    public Animator anim;
    public static bool open = false;
    public static bool idle = false;
    int IdleNoGun;
    int IdleGun;
    public static bool doing = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        anim = GetComponent<Animator>();
        anim.SetBool("state", true);
        IdleNoGun = Animator.StringToHash("Base Layer.idle without gun");
        IdleGun = Animator.StringToHash("Base Layer.idle with gun");
    }

    // Update is called once per frame
    void Update()
    {
        if ((anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleNoGun) || (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleGun))
        {
            doing = true;
        }
        else
        {
            doing = false;
        }
        //Debug.Log(doing);
        if (doing == true)
        {
            if (open == true)
            {
                anim.SetTrigger("open");
                open = false;
                StartCoroutine(stopping());
            }
        }
    }
    IEnumerator stopping()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        GunScript.open = false;
    }
}
