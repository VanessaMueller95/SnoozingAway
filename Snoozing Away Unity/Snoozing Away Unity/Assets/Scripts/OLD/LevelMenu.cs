using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {

    //Läd Level 1
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Cuboid");
    }

    //Läd das Hauptmenü
    public void LoadMainMenue()
    {
        SceneManager.LoadScene("Menu");
    }
}
