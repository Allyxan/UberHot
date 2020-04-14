using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterchangeDoor : MonoBehaviour
{
    Animator IntDoor;
    private bool IntOpened = false;
    void Start()
    {
        IntDoor = GetComponent<Animator>();
        IntDoor.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && IntOpened == false)
        {
            IntDoor.enabled = true;
            IntOpened = true;
            StartCoroutine(DisableAnimation());
        }
    }
    IEnumerator DisableAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        IntDoor.enabled = false;
    }
}
