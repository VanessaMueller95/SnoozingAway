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

    //Variablen Swipe Controls
    private Vector3 fp;   //Touch Anfangsposition
    private Vector3 lp;   //Touch Endposition
    private float dragDistance;  //minimale Laenge um Swipe auszulösen

    //Beim Start des Spiels ist die Perspektive "Front"
    void Start()
    {
        direction = directionArray[directionCounter];

        //Swipelaenge muss groeßer als 10% der Bildschirmbreite sein
        dragDistance = Screen.width * 10 / 100; 
    }


    void Update()
    {
    //Kameradrehung um 90 Grad nach links oder rechts durch horizontales Swipen
        
        //Registrierung eines Single Touch
        if (Input.touchCount == 1) 
        {
            //Speichern der Anfangs- und Endpostion des Touchs
            Touch touch = Input.GetTouch(0); 
            if (touch.phase == TouchPhase.Began) 
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) 
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) 
            {
                lp = touch.position; 

                //Checkt ob die Swipe Laenge groeßer als 10% der Bildschirmbreite ist
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    // Checkt ob Swipe horizonzal ist
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                    
                        //Checkt ob Swipe nach rechts oder links gezogen wurde
                        if ((lp.x > fp.x))  //Right swipe
                        {   
                            Debug.Log("Right Swipe");

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
                            if (GameObject.Find("FieldController").GetComponent<ArrowButtons>().actualButton != null)
                            {
                                GameObject.Find("FieldController").GetComponent<ArrowButtons>().OnClickDirectionButton(GameObject.Find("FieldController").GetComponent<ArrowButtons>().actualButton);
                            }



                        }
                        else //Left swipe
                        {   
                            Debug.Log("Left Swipe");

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
                    }
                }
            }
        }


        //Kameradrehung um 90 Grad nach links oder rechts durch Druck der Pfeiltasten

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