﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowField : MonoBehaviour {

    //Variable für den Character
    public GameObject chara;

    //Dropdown der Richtung für den Inspektor
    public enum Orientation { Left, Right, Backward, Forward }
    public Orientation orientation;

    private void Start()
    {
        chara = GameObject.Find("Character");
    }

    //Test auf Collision der Pfeilfelder und dem Character
    //Getestet wird ob der Mittelpunkt des Characters im Collider enthalten ist, um den Character immer am ca. gleichen Punkt zu drehen
    //egal ob er von vorne oder Hinten den Collider trifft 
    void Update () {
        //Character Mittelpunkt im Collider des Feldes?
        if (transform.GetComponent<Collider>().bounds.Contains(chara.transform.position))
        {
            //Anpassung des Vektors für den Raycast des Characters und Drehung des Charackters in die gewünschte Richtung
            if (orientation == Orientation.Forward)
            {
                Snoozer.spreadAngle = Quaternion.AngleAxis(30, new Vector3(0, 0, 1));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
            else if (orientation == Orientation.Right)
            {
                Snoozer.spreadAngle = Quaternion.AngleAxis(30, new Vector3(1, 0, 0));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            }
            else if (orientation == Orientation.Left)
            {
                Snoozer.spreadAngle = Quaternion.AngleAxis(-30, new Vector3(1, 0, 0));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, -180, transform.eulerAngles.z);
            }
            else if (orientation == Orientation.Backward)
            {
                Snoozer.spreadAngle = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }

        }
    }
}