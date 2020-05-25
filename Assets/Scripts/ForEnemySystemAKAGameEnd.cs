using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class ForEnemySystemAKAGameEnd : MonoBehaviour
{
    GameObject fadeToBlack;
    public GameObject MenuManager;
    public GameObject Player;
    public TriggerScript triggerScript;
    Animator ani;
    public AudioClip UberHot;
    public AudioSource audio;
    public bool oneTimeLaunch = false;
    public float forPitch;
    public AudioMixer master;

    void Start()
    {
        fadeToBlack = GameObject.Find("FadeToBlack");
        ani = fadeToBlack.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && triggerScript.dontFinishTheGame == false)
        {
            MenuManager.SetActive(false);
            Player.GetComponent<SuperHotScript>().enabled = false;
            Player.tag = "Untagged";
            Time.timeScale = 1;
            forPitch = Time.timeScale;
            master.SetFloat("masterPitch", forPitch);
            if (oneTimeLaunch == false)
                StartCoroutine(QuitWaiting());
            oneTimeLaunch = true;
        }
    }
    IEnumerator QuitWaiting()
    {
        audio.PlayOneShot(UberHot, 0.7F);
        ani.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(2.3f);
        SceneManager.LoadScene(0);
    }
}