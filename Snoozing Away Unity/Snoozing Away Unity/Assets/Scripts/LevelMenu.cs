using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {

    //Läd Level 1
    public void LoadLevel(int number)
    {
        SceneManager.LoadScene("level" + number);
    }

    //Läd das Hauptmenü
    public void LoadMainMenue()
    {
        SceneManager.LoadScene("Menu");
    }
}
