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
    void Start()
    {
        DoorRb = GetComponent<Rigidbody>();
        DoorRb.isKinematic = true;
        DoorHinge = GetComponent<HingeJoint>();
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
            breakingdoor = true;
            GunScript.open = true;
            HandScript.open = true;
            StartCoroutine(BreakDoor());
        }
    }

    IEnumerator BreakDoor()
    {
        yield return new WaitForSecondsRealtime(0.95f);
        DoorRb.isKinematic = false;
        Destroy(DoorHinge);
        DoorRb.AddForce(-transform.forward * 150);
        DoorRb.AddForce(-transform.forward * 1500 * Time.deltaTime);
        yield return new WaitForSecondsRealtime(1f);
        breakingdoor = false;
        Destroy(this);
    }
}