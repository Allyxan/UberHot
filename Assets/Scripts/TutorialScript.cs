using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TutorialScript : MonoBehaviour
{
    //public static TutorialScript instance;
    public GameObject Tutorial;
    public GameObject Player;
    public float open;
    public float forPitch;
    public AudioMixer master;
    public bool stop; public bool startSound = false;
    void Start()
    {
        open = PlayerPrefs.GetFloat("OneTimeOpen");
    }
    void Update()
    {
        if (Player.GetComponent<SuperHotScript>().enabled == false && stop == false)
        {
            Time.timeScale = 1;
            master.SetFloat("masterPitch", forPitch);
            forPitch = Time.timeScale;
        }

        if (open != 0 && stop == false)
        {
            Tutorial.SetActive(false);
            //master.SetFloat("masterPitch", forPitch);
            //forPitch = Time.timeScale;
            StartCoroutine(waitIfAlreadyOpened());
        }
        if (Tutorial.activeSelf == true)
        {
            Time.timeScale = 0;
            GetComponent<PauseGame>().enabled = false;
            Player.GetComponent<SuperHotScript>().enabled = false;
            Player.GetComponent<Controller>().enabled = false;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                open = 0;
                //open++;
#else
                open++;
#endif
                PlayerPrefs.SetFloat("OneTimeOpen", open);
                GetComponent<PauseGame>().enabled = true;
                Tutorial.SetActive(false);
                Time.timeScale = 1;
                Player.GetComponent<Controller>().enabled = true;
                StartCoroutine(waitingUntilDoors());
            }
        }
    }
    IEnumerator waitingUntilDoors()
    {
        startSound = true;
        yield return new WaitForSecondsRealtime(1f);
        Player.GetComponent<SuperHotScript>().enabled = true;
        stop = true;
    }
    IEnumerator waitIfAlreadyOpened()
    {
        startSound = true;
        Player.GetComponent<SuperHotScript>().enabled = false;
        yield return new WaitForSecondsRealtime(1f);
        Player.GetComponent<SuperHotScript>().enabled = true;
        stop = true;
    }
}