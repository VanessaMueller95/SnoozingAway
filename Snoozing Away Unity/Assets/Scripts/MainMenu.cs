using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //Level Starten
	public void PlayGame()
    {
        SceneManager.LoadScene("scene-newWorld");
    }

    //Spiel beenden
    public void QuitGame()
    {
        Application.Quit();
    }
}
