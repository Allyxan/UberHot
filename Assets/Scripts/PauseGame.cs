using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseGame : MonoBehaviour
{
    bool quitting = false;
    bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject crossHair;
    public GameObject Player;
    public float forPitch;
    public AudioMixer master;
    GameObject fadeToBlack;
    Animator ani;
    TutorialScript tutorialScript;

    void Start()
    {
        tutorialScript = GameObject.Find("MenuManager").GetComponent<TutorialScript>();
        fadeToBlack = GameObject.Find("FadeToBlack");
        ani = fadeToBlack.GetComponent<Animator>();
    }
    void Update()
    {
        if (gamePaused == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && quitting == false && tutorialScript.stop == true)
        {
            if (gamePaused == false)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                AudioListener.pause = true;
                Player.GetComponent<Controller>().enabled = false;
                Player.GetComponent<SuperHotScript>().enabled = false;
                gamePaused = true;
                crossHair.SetActive(false);
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        crossHair.SetActive(true);
        Player.GetComponent<Controller>().enabled = true;
        Player.GetComponent<SuperHotScript>().enabled = true;
        AudioListener.pause = false;
        Time.timeScale = 1;
        gamePaused = false;
    }
    public void Restart()
    {
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Player.GetComponent<SuperHotScript>().enabled = false;
        Player.tag = "Untagged";
        Time.timeScale = 1;
        forPitch = Time.timeScale;
        master.SetFloat("masterPitch", forPitch);
        AudioListener.pause = false;
        StartCoroutine(QuitWaiting());
    }
    IEnumerator QuitWaiting()
    {
        quitting = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        ani.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(0);
    }
}