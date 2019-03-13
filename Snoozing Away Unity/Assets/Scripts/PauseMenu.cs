using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //Variablen für den Status und das Pause UI Element
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;

    void Update () {
        //Pause Menü aktivieren oder deaktivieren durch die Taste P
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("pause");
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
		
	}

    //Aktivieren des Pause Menüs, Anhalten des Spiels
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPause = true;
    }

    //Deaktivieren des Pause Menüs, Fortsetzen des Spiels
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPause = false;
    }

    public void PauseButton()
    {
        if (GameIsPause)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    //Neustart des Levels
    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("scene-newWorld");
    }

    //Laden des Menüs
    public void LoadMenu()
    {

    }

    //Level beenden, Rückkehr in den Startbildschirm
    public void QuitGame()
    {
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
