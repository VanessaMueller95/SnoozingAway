using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject targetObject;
    private float targetAngle = 0;
    const float rotationAmount = 1.5f;
    public float rDistance = 1.0f;
    public float rSpeed = 1.0f;

    //Tracking der aktuellen Richtung
    string[] directionArray = new string[] {"Front","Right","Back","Left"};
    int directionCounter = 0;

    //Wird bei Drehung der Kamera aktualisiert
    public string direction;

    //Beim Start des Spiels ist die Perspektive "Front"
    void Start()
    {
        direction = directionArray[directionCounter];
    }


    void Update()
    {
        //Bei Druck der Pfeiltasten wird die Perspektive um 90 Grad nach links oder rechts gedreht 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Festlegen des Zielwinkels (Wie viel muss rotiert werden)
            targetAngle -= 90.0f;

            //Tracking der Perspektive für die Verwendung in anderen Scripten, Linksklick reduziert den Counter für das Richtungs-Array
            directionCounter--;

            //Erreicht der Index des Arrays 0, wird auf den letzten Eintrag gesprungen
            if (directionCounter < 0)
            {
                directionCounter = 3;
            }

            direction = directionArray[directionCounter];
            Debug.Log(direction);

            //Aktualisiert die Prefabs der Pfeilfelder passend zur neuen Perspektive
            if(GameObject.Find("FieldController").GetComponent<ArrowButtons>().actualButton != null)
            {
                GameObject.Find("FieldController").GetComponent<ArrowButtons>().OnClickDirectionButton(GameObject.Find("FieldController").GetComponent<ArrowButtons>().actualButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Selbes Vorgehen nur wird das Feld nach Rechts gedreht und des Counter des Richtungs-Arrays wird um eins erhöht
            targetAngle += 90.0f;

            directionCounter++;

            if (directionCounter > 3)
            {
                directionCounter = 0;
            }

            direction = directionArray[directionCounter];
            Debug.Log(direction);

            //Aktualisiert die Prefabs der Pfeilfelder passend zur neuen Perspektive
            if (GameObject.Find("FieldController").GetComponent<ArrowButtons>().actualButton != null)
            {
                GameObject.Find("FieldController").GetComponent<ArrowButtons>().OnClickDirectionButton(GameObject.Find("FieldController").GetComponent<ArrowButtons>().actualButton);
            }
        }

        //Solange des Zielwinkel nicht dem aktuellen Winkel entspricht wird die Kamera rotiert
        if (targetAngle != 0)
        {
            Rotate();
        }
    }

    //Rotation
    protected void Rotate()
    {
        //float step = rSpeed * Time.deltaTime;
        //float orbitCircumfrance = 2F * rDistance * Mathf.PI;
        //float distanceDegrees = (rSpeed / orbitCircumfrance) * 360;
        //float distanceRadians = (rSpeed / orbitCircumfrance) * 2 * Mathf.PI;

        if (targetAngle > 0)
        {
            transform.RotateAround(targetObject.transform.position, Vector3.up, -rotationAmount);
            targetAngle -= rotationAmount;
        }
        else if (targetAngle < 0)
        {
            transform.RotateAround(targetObject.transform.position, Vector3.up, rotationAmount);
            targetAngle += rotationAmount;
        }
    }
}