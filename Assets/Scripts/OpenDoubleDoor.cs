using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoubleDoor : MonoBehaviour
{
    Animator DoubleDoor;
    private bool DoubleOpened = false;
    void Start()
    {
        DoubleDoor = GetComponent<Animator>();
        DoubleDoor.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && DoubleOpened == false)
        {
            DoubleDoor.enabled = true;
            DoubleOpened = true;
            StartCoroutine(DisableAnimation());
        }
    }
    IEnumerator DisableAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        DoubleDoor.enabled = false;
    }
}