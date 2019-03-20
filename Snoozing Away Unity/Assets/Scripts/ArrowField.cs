using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowField : MonoBehaviour {

    //Variable für den Character
    public GameObject chara;

    //Dropdown der Lauf-Richtung für den Inspektor
    public enum Orientation { Left, Right, Backward, Forward }
    public Orientation orientation;

    private void Start()
    {
        chara = GameObject.Find("Character");
    }

    //Test auf Collision zwischen den Pfeilfelder und dem Character
    //Getestet wird ob der Mittelpunkt des Characters im Collider enthalten ist, um den Character immer am ca. gleichen Punkt zu drehen, egal aus welcher Richtung er kommt
    void Update () {
        //Character Mittelpunkt im Collider des Feldes?
        if (transform.GetComponent<Collider>().bounds.Contains(chara.transform.position))
        {
            //Anpassung des Vektors für den Raycast des Charackters und Drehung des Charackters in die gewünschte Richtung
            if (orientation == Orientation.Forward)
            {
                chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(4, new Vector3(0, 0, 1));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
            else if (orientation == Orientation.Right)
            {
                chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(4, new Vector3(1, 0, 0));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            }
            else if (orientation == Orientation.Left)
            {
                chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(-4, new Vector3(1, 0, 0));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, -180, transform.eulerAngles.z);
            }
            else if (orientation == Orientation.Backward)
            {
                chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(-4, new Vector3(0, 0, 1));
                chara.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }

        }
    }
}
