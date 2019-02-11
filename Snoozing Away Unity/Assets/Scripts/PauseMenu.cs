using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    // Use this for initialization
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
	
	// Update is called once per frame
	void Update () {
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPause = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("scene-newWorld");
    }

    public void LoadMenu()
    {

    }

    public void QuitGame()
    {
        GameIsPause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
