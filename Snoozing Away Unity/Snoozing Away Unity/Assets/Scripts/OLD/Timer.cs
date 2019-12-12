using System.Collections;
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

    public bool colorChanged = false;

    public Camera cam;

    private Animator animator;



    Text text;

    private void Start()
    {
        cam = Camera.main.GetComponent<Camera>();
        animator = GetComponent<Animator>();
    }

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

        if (targetTime <= 85.0f && colorChanged == false)
        {
            //uiText.color = new Color(193f/255.0f, 86f/255.0f, 86f/255.0f);
            colorChanged = true;
            StartCoroutine(UpdateTextColor());
            Debug.Log("In methode");
        }

        if (targetTime <= 85.0f && timerActive)
        {
            animator.SetBool("active", true);
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
        //FindObjectOfType<AudioManager>().Stop("Ticking");
        //FindObjectOfType<AudioManager>().Play("Ring");
        timerActive = false;
        Time.timeScale = 0f;
        restartMenuUI.SetActive(true);
    }

    public IEnumerator UpdateTextColor()
    {
        Debug.Log("Test1");
        float t = 0;
        while (t < 1)
        {
            Debug.Log("Test");
            // Now the loop will execute on every end of frame until the condition is true
            uiText.color = Color.Lerp(new Color(1f, 1f, 1f), new Color(193f / 255.0f, 86f / 255.0f, 86f / 255.0f), t);

            cam.backgroundColor = Color.Lerp(new Color(183f/255f, 183f / 255f, 183f / 255f), new Color(200f / 255.0f, 158f / 255.0f, 158f / 255.0f), t);

            t += Time.deltaTime / 5f;
            yield return new WaitForEndOfFrame(); // So that I return something at least.
        }
    }

}
