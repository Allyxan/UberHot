using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAdmDoorScript : MonoBehaviour
{
    public new Camera camera;
    //public LayerMask doorLayer;
    Rigidbody DoorRb;
    HingeJoint DoorHinge;

    public AudioSource ZvukDver;

    void Start()
    {
        DoorRb = GetComponent<Rigidbody>();
        DoorRb.isKinematic = true;
        DoorHinge = GetComponent<HingeJoint>();
        ZvukDver = GetComponent<AudioSource>();
    }
    /*void Update()
    {
        RaycastHit hitDoor;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitDoor, 3, doorLayer))
        {
            if (Input.GetKeyDown(KeyCode.E) && LayerMask.LayerToName(14) == "AdmDoor")
            {
                BreakDoor();
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BreakDoor();
            ZvukDver.Play();
        }
    }

    public void BreakDoor()
    {
        DoorRb.isKinematic = false;
        Destroy(DoorHinge);
        DoorRb.AddForce(-transform.forward * 150);
        DoorRb.AddForce(-transform.forward * 1500 * Time.deltaTime);
        Destroy(this);
    }
}