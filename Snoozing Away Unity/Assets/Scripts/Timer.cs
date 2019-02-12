using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text uiText;
    public float targetTime = 60.0f;
    bool timerActive = true;
    public GameObject restartMenuUI;

    void Update()
    {
        if (targetTime >= 0.0f && timerActive == true)
        {
            targetTime -= Time.deltaTime;
            uiText.text = "" + (int)targetTime;
        }
        

        if (targetTime <= 0.0f && timerActive == true)
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
        timerActive = false;
        Time.timeScale = 0f;
        restartMenuUI.SetActive(true);
    }

}
