using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    //UI Element für die Anzeige
    public Text uiText;

    //Zeitlimit, kann im Editor geändert werden
    public float targetTime = 100.0f;

    //Zum Aktivieren oder Deaktivieren
    public bool timerActive = false;

    //Übergabe des MenüObjekts, damit es nach Wunsch aktiviert werden kann
    public GameObject restartMenuUI;

    void Update()
    {
        //Abfrage nach dem Status und der Verbleibenden Zeit 
        if (targetTime >= 0.0f && timerActive == true)
        {
            //Runterzählen und Aktualisierung des UI Textes
            targetTime -= Time.deltaTime;
            uiText.text = "" + (int)targetTime;
        }
        
        //Beenden des Timers
        if (targetTime <= 0.0f && timerActive == true)
        {
            timerEnd();
        }

    }

    //Funktion zum aktivieren
    void timerStart()
    {
        timerActive = true;
    }

    //Funktion zum beenden
    void timerEnd()
    {
        //Status wird aktualisiert, die Zeit angehalten und das Menü aktiviert
        FindObjectOfType<AudioManager>().Stop("Ticking");
        FindObjectOfType<AudioManager>().Play("Ring");
        timerActive = false;
        Time.timeScale = 0f;
        restartMenuUI.SetActive(true);
    }

}
