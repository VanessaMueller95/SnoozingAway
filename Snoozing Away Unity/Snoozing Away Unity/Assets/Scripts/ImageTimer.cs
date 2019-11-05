using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{

    //Zum Aktivieren oder Deaktivieren
    public bool timerActive = false;

    //Übergabe des MenüObjekts, damit es nach Wunsch aktiviert werden kann
    public GameObject restartMenuUI;

    Image fillImg;
    public float timeAmt = 100;
    float time;

    // Use this for initialization
    void Start()
    {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && timerActive == true)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
        }
        else if (time <= 0 && timerActive == true)
        {
            timerEnd();
        }
    }

    void timerStart()
    {
        timerActive = true;
    }

    void timerEnd()
    {
        //Status wird aktualisiert, die Zeit angehalten und das Menü aktiviert
        //FindObjectOfType<AudioManager>().Stop("Ticking");
        //FindObjectOfType<AudioManager>().Play("Ring");
        timerActive = false;
        Time.timeScale = 0f;
        restartMenuUI.SetActive(true);
    }
}