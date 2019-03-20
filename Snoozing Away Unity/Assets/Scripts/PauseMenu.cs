using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //Variablen für den Status und das Pause UI Element
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public GameObject helpMenuUI;

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
        FindObjectOfType<AudioManager>().Stop("Ticking");
    }

    public void OpenHelp()
    {
        helpMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPause = true;
        FindObjectOfType<AudioManager>().Stop("Ticking");
    }

    //Deaktivieren des Pause Menüs, Fortsetzen des Spiels
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPause = false;
        FindObjectOfType<AudioManager>().Play("Ticking");
    }

    public void ResumeFromHelp()
    {
        helpMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPause = false;
        FindObjectOfType<AudioManager>().Play("Ticking");
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
        FindObjectOfType<AudioManager>().Stop("Ticking");
        FindObjectOfType<AudioManager>().Stop("Ring");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("level1");
    }

    //Laden des Menüs
    public void LoadLevelMenue()
    {
        FindObjectOfType<AudioManager>().Stop("Ring");
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("LevelMenue");
    }

    //Level beenden, Rückkehr in den Startbildschirm
    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Stop("Ring");
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
