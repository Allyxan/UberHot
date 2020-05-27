using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAdmDoorScript : MonoBehaviour
{
    public static bool breakingdoor = false;
    public new Camera camera;
    //public LayerMask doorLayer;
    Rigidbody DoorRb;
    HingeJoint DoorHinge;
    public AudioSource ZvukDver;
    public GameObject weaponHolder;
    public bool checkComplete = false;
    public bool inTrigger = false;
    public bool oneTimeOpen = false;

    void Start()
    {
        DoorRb = GetComponent<Rigidbody>();
        DoorRb.isKinematic = true;
        DoorHinge = GetComponent<HingeJoint>();
        ZvukDver = GetComponent<AudioSource>();
    }
    void Update()
    {
        RaycastHit doorCheck;
        if (Physics.Raycast(weaponHolder.transform.position, weaponHolder.transform.forward, out doorCheck, 1))
        {
            if (doorCheck.transform.CompareTag("ResseptionDoor") && inTrigger == true)
                checkComplete = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            inTrigger = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && checkComplete && !oneTimeOpen)
        {
            breakingdoor = true;
            GunScript.open = true;
            HandScript.open = true;
            StartCoroutine(BreakDoor());
            oneTimeOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            inTrigger = false;
    }
    IEnumerator BreakDoor()
    {
        yield return new WaitForSeconds(0.95f);
        ZvukDver.Play();
        DoorRb.isKinematic = false;
        Destroy(DoorHinge);
        DoorRb.AddForce(-transform.forward * 150);
        DoorRb.AddForce(-transform.forward * 1000 * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        breakingdoor = false;
        yield return new WaitForSeconds(1f);
        DoorRb.isKinematic = true;
        Destroy(this);
    }
}