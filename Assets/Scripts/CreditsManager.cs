using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsManager : MonoBehaviour
{
    bool checkTime = false;
    public GameObject MainMenu;
    public GameObject OptionsPanel;
    public GameObject CreditsPanel;
    GameObject fadeToCredits;
    Animator ani;
    void Start()
    {
        fadeToCredits = GameObject.Find("FadeToCredits");
        ani = fadeToCredits.GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && MainMenu.activeSelf == false)
        {
            CreditsPanel.SetActive(false);
            MainMenu.SetActive(true);
            OptionsPanel.SetActive(true);
            ani.SetTrigger("Back");
            StopAllCoroutines();
        }
    }
    public void ToCredits()
    {
        StopAllCoroutines();
        StartCoroutine(Credits());
    }
    IEnumerator Credits()
    {
        checkTime = true;
        ani.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(1f);
        ani.SetTrigger("Back");
        MainMenu.SetActive(false);
        OptionsPanel.SetActive(false);
        CreditsPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        CreditsPanel.SetActive(false);
        MainMenu.SetActive(true);
        OptionsPanel.SetActive(true);
    }
}