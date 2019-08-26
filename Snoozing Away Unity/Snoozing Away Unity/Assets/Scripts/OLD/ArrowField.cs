using UnityEngine;

public class ArrowField : MonoBehaviour {

    //Variable für den Character
    public GameObject chara;

    //Dropdown der Lauf-Richtung für den Inspektor
    public enum Orientation { Left, Right, Backward, Forward }
    public Orientation orientation;
    private string lastRotation = null;

    private void Start()
    {
        chara = GameObject.Find("Snoozer");
    }

    //Test auf Collision zwischen den Pfeilfelder und dem Character
    //Getestet wird ob der Mittelpunkt des Characters im Collider enthalten ist, um den Character immer am ca. gleichen Punkt zu drehen, egal aus welcher Richtung er kommt
    void Update () {
        //Character Mittelpunkt im Collider des Feldes?
        if (transform.GetComponent<Collider>().bounds.Contains(chara.transform.position))
        {
            //Debug.Log("TEEEEEST");
            //Anpassung des Vektors für den Raycast des Charackters und Drehung des Charackters in die gewünschte Richtung
            if (orientation == Orientation.Forward)
            {
                //Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, -transform.right) * chara.transform.rotation;
                //chara.transform.rotation = newRotation;

                //chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(4, new Vector3(0, 0, 1));
                //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, 90, chara.transform.eulerAngles.z);
                //chara.transform.Rotate(new Vector3(0, 1, 0), Space.World);
                //chara.transform.rotation = Quaternion.Euler(0, 90, 0);
                //chara.transform.localRotation = Quaternion.Euler(1, 1, 1);
                if (chara.GetComponent<WalkNew>().lastRotation != "forward")
                {
                    chara.GetComponent<WalkNew>().lastRotation = "forward";
                    Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, -transform.right) * chara.transform.rotation;
                    chara.transform.rotation = newRotation;
                    //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, 90, chara.transform.eulerAngles.z);

                }
                Debug.Log("In Forward");
            }
            else if (orientation == Orientation.Right)
            {
                //Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, -transform.forward) * chara.transform.rotation;
                //chara.transform.rotation = newRotation;
                //chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(4, new Vector3(1, 0, 0));
                //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, 180, chara.transform.eulerAngles.z);
                //chara.transform.rotation = Quaternion.Euler(chara.transform.eulerAngles.x, 180, chara.transform.eulerAngles.z);
                if (chara.GetComponent<WalkNew>().lastRotation != "right")
                {
                    chara.GetComponent<WalkNew>().lastRotation = "right";
                    Debug.Log("In Right");

                    Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, -transform.forward) * chara.transform.rotation;
                    chara.transform.rotation = newRotation;
                    //chara.transform.rotation = Quaternion.Euler(chara.transform.eulerAngles.x, 180, chara.transform.eulerAngles.z);
                }
                
                
            }
            else if (orientation == Orientation.Left)
            {
                //Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, -transform.forward) * chara.transform.rotation;
                //chara.transform.rotation = newRotation;
                //chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(-4, new Vector3(1, 0, 0));
                //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, 0, chara.transform.eulerAngles.z);
                if (chara.GetComponent<WalkNew>().lastRotation != "left")
                {
                    chara.GetComponent<WalkNew>().lastRotation = "left";
                    Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, transform.forward) * chara.transform.rotation;
                    chara.transform.rotation = newRotation;
                    //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, 0, chara.transform.eulerAngles.z);
                }
                Debug.Log("In Left");
            }
            else if (orientation == Orientation.Backward)
            {
                //Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, transform.right) * chara.transform.rotation;
                //chara.transform.rotation = newRotation;

                //chara.GetComponent<Snoozer>().spreadAngle = Quaternion.AngleAxis(-4, new Vector3(0, 0, 1));
                //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, 90, chara.transform.eulerAngles.z);
                if (chara.GetComponent<WalkNew>().lastRotation != "backward")
                {
                    chara.GetComponent<WalkNew>().lastRotation = "backward";
                    Quaternion newRotation = Quaternion.FromToRotation(chara.transform.forward, transform.right) * chara.transform.rotation;
                    chara.transform.rotation = newRotation;
                    //chara.transform.eulerAngles = new Vector3(chara.transform.eulerAngles.x, -90, chara.transform.eulerAngles.z);
                }
                Debug.Log("In Backward");
            }

        }
    }
}
