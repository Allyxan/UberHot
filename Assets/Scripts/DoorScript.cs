using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    int OpenCounter;
    public string nameTrigger;
    Animator DoorAnim;
    private void Start()
    {
        DoorAnim = GetComponent<Animator>();
        DoorAnim.enabled = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && OpenCounter == 0)
        {
            StartCoroutine(DisableDoorAnimation());
        }
    }
    IEnumerator DisableDoorAnimation()
    {
        DoorAnim.enabled = true;
        OpenCounter++;
        yield return new WaitForSeconds(1.0f);
        StopAllCoroutines();
        DoorAnim.enabled = false;
    }

}
