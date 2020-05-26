using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;
    public bool breakResseptionDoor = false;
    public new Camera camera;
    public LayerMask doorLayer;
    Rigidbody DoorRb;
    HingeJoint DoorHinge;

    //public AudioClip ZvukDver;
    public AudioSource Fonovaya;
    //public AudioSource audio;

    void Start()
    {
        instance = this;
        DoorRb = GetComponent<Rigidbody>();
        DoorRb.isKinematic = true;
        DoorHinge = GetComponent<HingeJoint>();
        Fonovaya = GetComponent<AudioSource>();
    }
    /*void Update()
    {
        RaycastHit hitDoor;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitDoor, 3, doorLayer))
        {
            if (Input.GetKeyDown(KeyCode.E) && LayerMask.LayerToName(12) == "DoorRes")
            {
                BreakDoor();
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && breakResseptionDoor == false)
        {
            BreakDoor();
            //audio.PlayOneShot(ZvukDver, 0.7F);
            Fonovaya.Play();
        }
    }

    public void BreakDoor()
    {
        DoorRb.isKinematic = false;
        Destroy(DoorHinge);
        DoorRb.AddForce(-transform.forward * 150);
        DoorRb.AddForce(-transform.forward * 1500 * Time.deltaTime);
        Destroy(this);
        breakResseptionDoor = true;
    }
}
