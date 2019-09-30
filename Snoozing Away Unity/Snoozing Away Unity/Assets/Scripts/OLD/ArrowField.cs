using UnityEngine;

public class ArrowField : MonoBehaviour {

    //Variable für den Character
    public GameObject chara;

    //Dropdown der Lauf-Richtung für den Inspektor
    public enum Orientation { Left, Right, Backward, Forward }
    public Orientation orientation;

    private void Start()
    {
        chara = GameObject.Find("Snoozer");
    }

    //Test auf Collision zwischen den Pfeilfelder und dem Character
    //Getestet wird ob der Mittelpunkt des Characters im Collider enthalten ist, um den Character immer am ca. gleichen Punkt zu drehen, egal aus welcher Richtung er kommt
    void Update () {
        //Debug.Log("PFEILFELD NORMALE:" + transform.up);
        //Character Mittelpunkt im Collider des Feldes?
        if (transform.GetComponent<Collider>().bounds.Contains(chara.transform.position))
        {
            //Anpassung des Vektors für den Raycast des Charackters und Drehung des Charackters in die gewünschte Richtung
            if (orientation == Orientation.Forward)
            {
                chara.transform.rotation = Quaternion.LookRotation(-transform.right, chara.transform.up);
            }
            else if (orientation == Orientation.Right)
            {
                chara.transform.rotation = Quaternion.LookRotation(-transform.forward, chara.transform.up);
            }
            else if (orientation == Orientation.Left)
            {
                chara.transform.rotation = Quaternion.LookRotation(transform.forward, chara.transform.up);
            }
            else if (orientation == Orientation.Backward)
            {
                chara.transform.rotation = Quaternion.LookRotation(transform.right, chara.transform.up);
            }

        }
    }
}
