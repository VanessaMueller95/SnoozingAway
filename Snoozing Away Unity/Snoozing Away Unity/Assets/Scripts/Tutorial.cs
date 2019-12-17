using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private GameObject[] tutorials;
    public int activeTutorial;
    private bool tutorialFinished = false;

    //alle aktiven Tutorials werden in ein Array aufgenommen
    void Start()
    {
        activeTutorial = 0;
        tutorials = new GameObject[transform.childCount];
        var i = 0;
        foreach (Transform child in transform)
        {
            tutorials[i] = child.gameObject;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //wenn das Tutorial noch läuft
        if (!tutorialFinished)
        {
            //dient dazu das ganze auszublenden wenn Menüs aktiv sind -> überarbeiten
            if(Time.timeScale == 0f)
            {
                tutorials[activeTutorial].SetActive(false);
            }
            //aktiviert aktuelles Tutorial, wenn es nicht aktiv ist
            else
            {
                if (!tutorials[activeTutorial].activeSelf)
                {
                    tutorials[activeTutorial].SetActive(true);
                }
            }
        }
    }

    //startet durch den Buttonklick die nächste Tutorial Stufe
    public void nextTutorial()
    {
        tutorials[activeTutorial].SetActive(false);
        activeTutorial++;
        //sind alle durchlaufen wird das Tutorial beendet
        if (activeTutorial > transform.childCount - 1)
        {
            tutorialFinished = true;
        }
        else {
            tutorials[activeTutorial].SetActive(true);
        }
    }
}
