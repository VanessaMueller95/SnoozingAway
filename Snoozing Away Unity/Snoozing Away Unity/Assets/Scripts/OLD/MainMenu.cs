
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    //Variablen für den Audiomanager und die Volume Slider
    public AudioManager audioManager;
    public Slider sliderMainvolume;
    public Slider sliderEffectsvolume;

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        sliderMainvolume.value = audioManager.GetComponent<AudioManager>().MasterVolume;
        sliderEffectsvolume.value = audioManager.GetComponent<AudioManager>().EffectsVolume;
    }

    //Levelmenü laden
    public void PlayGame()
    {
        SceneManager.LoadScene("levelMenu");
    }

    //Spiel beenden
    public void QuitGame()
    {
        Application.Quit();
    }

    //Anpassen des Master-Volumes
    public void SetMastervolume ()
    {
        audioManager.GetComponent<AudioManager>().MasterVolume = sliderMainvolume.value;
    }

    //Anpassen des Effects-Volumes
    public void SetEffectsvolume()
    {
        audioManager.GetComponent<AudioManager>().EffectsVolume = sliderEffectsvolume.value;
    }
}
