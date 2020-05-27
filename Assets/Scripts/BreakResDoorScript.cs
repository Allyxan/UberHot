using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakResDoorScript : MonoBehaviour
{
    //int layerMask;
    public static bool breakingdoor = false;
    public static BreakResDoorScript instance;
    public bool breakResseptionDoor = false;
    public new Camera camera;
    public LayerMask doorLayer;
    Rigidbody DoorRb;
    HingeJoint DoorHinge;
    public AudioClip BreakDoorSound;
    public AudioSource audio;
    public GameObject weaponHolder;
    public bool checkComplete = false;
    public bool inTrigger = false;
    public bool oneTimeOpen = false;
    void Start()
    {
        //layerMask = 1 << 12;
        audio = GetComponent<AudioSource>();
        instance = this;
        DoorRb = GetComponent<Rigidbody>();
        DoorRb.isKinematic = true;
        DoorHinge = GetComponent<HingeJoint>();
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
        if(other.tag == "Player" && breakResseptionDoor == false)
            inTrigger = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && breakResseptionDoor == false && checkComplete && !oneTimeOpen)
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
        if (other.tag == "Player" && breakResseptionDoor == false)
            inTrigger = false;
    }
    IEnumerator BreakDoor()
    {
        yield return new WaitForSeconds(0.95f);
        audio.PlayOneShot(BreakDoorSound, 0.7F);
        DoorRb.isKinematic = false;
        Destroy(DoorHinge);
        DoorRb.AddForce(-transform.forward * 150);
        DoorRb.AddForce(-transform.forward * 1500 * Time.deltaTime);
        yield return new WaitForSeconds(0.6f);
        breakingdoor = false;
        breakResseptionDoor = true;
        DoorRb.isKinematic = true;
        Destroy(this);
    }
}
