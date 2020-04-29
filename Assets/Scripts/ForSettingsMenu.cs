using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class ForSettingsMenu : MonoBehaviour
{
    public Slider masterVolume;
    public Slider musicVolume;
    public AudioMixer audioMixer;
    void Start()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        musicVolume.onValueChanged.AddListener(SetMusicVolume);
        //masterVolume.value = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : volume;
        //musicVolume.value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : volume;
    }
    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume*2/3);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume*2/3);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}