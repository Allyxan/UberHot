using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject Tutorial;
    public GameObject Player;
    public float open;
    bool stop;
    void Start()
    {
        open = PlayerPrefs.GetFloat("OneTimeOpen");
    }
    void Update()
    {
        if (open != 0 && stop == false)
        {
            Time.timeScale = 1;
            Tutorial.SetActive(false);
            StartCoroutine(waitIfAlreadyOpened());
            stop = true;
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
        yield return new WaitForSecondsRealtime(1f);
        Player.GetComponent<SuperHotScript>().enabled = true;
    }
    IEnumerator waitIfAlreadyOpened()
    {
        Player.GetComponent<SuperHotScript>().enabled = false;
        yield return new WaitForSecondsRealtime(1f);
        Player.GetComponent<SuperHotScript>().enabled = true;
    }
}