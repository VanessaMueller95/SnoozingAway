using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {

    //Läd den ausgewählten Level
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
