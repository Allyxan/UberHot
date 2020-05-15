using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ForEnemySystemAKAGameEnd : MonoBehaviour
{
    GameObject fadeToBlack;
    Animator ani;
    public AudioClip UberHot;
    public AudioSource audio;
    void Start()
    {
        fadeToBlack = GameObject.Find("FadeToBlack");
        ani = fadeToBlack.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Time.timeScale = 1;
            StartCoroutine(QuitWaiting());
            
        }
    }
    IEnumerator QuitWaiting()
    {
        audio.PlayOneShot(UberHot, 0.7F);
        ani.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(0);
        
    }
}