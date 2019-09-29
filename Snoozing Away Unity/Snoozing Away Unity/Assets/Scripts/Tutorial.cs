using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private GameObject[] tutorials;
    public int activeTutorial;
    private bool tutorialFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        activeTutorial = 0;
        tutorials = new GameObject[transform.childCount];
        var i = 0;
        foreach (Transform child in transform)
        {
            tutorials[i] = child.gameObject;
            Debug.Log(tutorials[i]);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialFinished)
        {
            if (Time.timeScale == 0f)
            {
                tutorials[activeTutorial].SetActive(false);
            }
            else
            {
                if (!tutorials[activeTutorial].activeSelf)
                {
                    tutorials[activeTutorial].SetActive(true);
                }
            }
        }
    }

    public void nextTutorial()
    {
        tutorials[activeTutorial].SetActive(false);
        activeTutorial++;
        if (activeTutorial > transform.childCount - 1)
        {
            tutorialFinished = true;
        }
        else {
            tutorials[activeTutorial].SetActive(true);
        }
    }
}
