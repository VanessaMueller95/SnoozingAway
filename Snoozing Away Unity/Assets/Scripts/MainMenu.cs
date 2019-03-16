using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public AudioManager audioManager;
    public Slider sliderMainvolume;
    public Slider sliderEffectsvolume;

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        sliderMainvolume.value = audioManager.GetComponent<AudioManager>().MasterVolume;
        sliderEffectsvolume.value = audioManager.GetComponent<AudioManager>().EffectsVolume;
    }

    //Level Starten
    public void PlayGame()
    {
        SceneManager.LoadScene("levelMenue");
    }

    //Spiel beenden
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetMastervolume ()
    {
        //audioManager = FindObjectOfType<AudioManager>();
        audioManager.GetComponent<AudioManager>().MasterVolume = sliderMainvolume.value;
    }

    public void SetEffectsvolume()
    {
        //audioManager = FindObjectOfType<AudioManager>();
        audioManager.GetComponent<AudioManager>().EffectsVolume = sliderEffectsvolume.value;
    }
}
