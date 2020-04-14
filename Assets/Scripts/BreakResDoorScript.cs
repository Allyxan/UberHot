using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakResDoorScript : MonoBehaviour
{
    public static BreakResDoorScript instance;
    public bool breakResseptionDoor = false;
    public new Camera camera;
    public LayerMask doorLayer;
    Rigidbody DoorRb;
    HingeJoint DoorHinge;
    void Start()
    {
        instance = this;
        DoorRb = GetComponent<Rigidbody>();
        DoorHinge = GetComponent<HingeJoint>();
    }
    void Update()
    {
        RaycastHit hitDoor;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitDoor, 3, doorLayer))
        {
            if (Input.GetKeyDown(KeyCode.E) && LayerMask.LayerToName(12) == "DoorRes")
            {
                BreakDoor();
            }
        }
    }
    public void BreakDoor()
    {
        Destroy(DoorHinge);
        DoorRb.AddForce(-transform.forward * 150);
        DoorRb.AddForce(-transform.forward * 1500 * Time.deltaTime);
        Destroy(this);
        breakResseptionDoor = true;
    }
}
