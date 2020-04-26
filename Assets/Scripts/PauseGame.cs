using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject crossHair;
    public GameObject Player;
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
                pauseMenu.SetActive(true);
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
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
        AudioListener.pause = false;
        Time.timeScale = 1;
#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
        SceneManager.LoadScene(0);
#else
        SceneManager.LoadScene(0);
#endif
    }
}