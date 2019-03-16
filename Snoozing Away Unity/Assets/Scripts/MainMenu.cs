using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public AudioManager audioManager;
    public Slider slider;

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        slider.value = audioManager.GetComponent<AudioManager>().MasterVolume;
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
        audioManager.GetComponent<AudioManager>().MasterVolume = slider.value;
    }
}
