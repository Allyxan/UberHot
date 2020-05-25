using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSounds : MonoBehaviour
{
    public AudioSource ZvukDver;
    TutorialScript tutorialScript;
    public GameObject Tutorial;
    public bool stopNow = false;
    void Start()
    {
        tutorialScript = GameObject.Find("MenuManager").GetComponent<TutorialScript>();
        ZvukDver = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Tutorial.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ZvukDver.Play();
                stopNow = true;
            }
        }
        if (tutorialScript.open != 0 && tutorialScript.startSound == true && stopNow == false)
        {
            ZvukDver.Play();
            stopNow = true;
        }
    }
    //void OpenDoor()
    //{
    //   ZvukDver.Play();
    //}
}
