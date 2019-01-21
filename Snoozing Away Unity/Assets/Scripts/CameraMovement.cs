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

    string[] directionArray = new string[] {"Front","Right","Back","Left"};
    int directionCounter = 0;

    public string direction;

    void Start()
    {
           direction = directionArray[directionCounter];
    }
    
    // Update is called once per frame
    void Update()
    {

        // Trigger functions if Rotate is requested
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetAngle -= 90.0f;
            directionCounter--;
            if (directionCounter > 3)
            {
                directionCounter = 0;
            }
            else if (directionCounter < 0)
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
            targetAngle += 90.0f;
            directionCounter++;
            if (directionCounter > 3)
            {
                directionCounter = 0;
            }
            else if (directionCounter < 0)
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

        if (targetAngle != 0)
        {
            Rotate();
        }
    }

    protected void Rotate()
    {
        float step = rSpeed * Time.deltaTime;
        float orbitCircumfrance = 2F * rDistance * Mathf.PI;
        float distanceDegrees = (rSpeed / orbitCircumfrance) * 360;
        float distanceRadians = (rSpeed / orbitCircumfrance) * 2 * Mathf.PI;

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