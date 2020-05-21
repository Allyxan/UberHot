using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;
    public Animator anim;
    public static bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        anim = GetComponent<Animator>();
        anim.SetBool("state", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (open == true)
        {
            anim.SetTrigger("open");
            HandScript.instance.anim.SetTrigger("open");
            open = false;
        }
    }
}
