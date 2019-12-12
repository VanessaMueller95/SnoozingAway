using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //Variablen für den Status und die Pause UI Elemente
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

    //Aktivieren des Pause Menüs, Anhalten des Spiels, Ticken-Sound stoppen
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPause = true;
        FindObjectOfType<AudioManager>().Stop("Ticking");
    }

    //Deaktivieren des Pause Menüs, Fortsetzen des Spiels, Ticken-Sound starten
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPause = false;
        FindObjectOfType<AudioManager>().Play("Ticking");
    }

    //Öffnen der Hilfe
    public void OpenHelp()
    {
        pauseMenuUI.SetActive(false);
        helpMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPause = true;
        FindObjectOfType<AudioManager>().Stop("Ticking");
    }

    //Schließen der Hilfe
    public void ResumeFromHelp()
    {
        helpMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPause = false;
        FindObjectOfType<AudioManager>().Play("Ticking");
    }

    //Funktion für den Pause Icon der Mobilen Version
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
        var Level = GameObject.Find("Cuboid").GetComponent<Cuboid>().levelNumber;
        SceneManager.LoadScene("level" + Level);
    }

    //Laden der Levelauswahl
    public void LoadLevelMenue()
    {
        FindObjectOfType<AudioManager>().Stop("Ring");
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("LevelMenu");
    }

    //Level beenden, Rückkehr in den Startbildschirm
    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Stop("Ring");
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        FindObjectOfType<AudioManager>().Stop("Ring");
        var nextLevel = GameObject.Find("Cuboid").GetComponent<Cuboid>().levelNumber +1;
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("level" + nextLevel);
    }
}
