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


    Vector2 firstPressPos;
    Vector2 currentSwipe;

    // Update is called once per frame
    void Update()
    {

        //Swipe
        /*if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentSwipe = (Vector2)Input.mousePosition - firstPressPos;

            Debug.Log(currentSwipe + ", " + currentSwipe.normalized);
        }

        float disX = Mathf.Abs(firstPressPos.x - currentSwipe.x);
        float disY = Mathf.Abs(firstPressPos.y - currentSwipe.y);*/



        // Trigger functions if Rotate is requested
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetAngle -= 90.0f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetAngle += 90.0f;
        }

        if (targetAngle != 0)
        {
            Rotate();
        }


        /*    if (disX > 0 || disY > 0)
            {
                if (disX > disY)
                {
                    targetAngle -= 90.0f;
                    disX = -1;
                    disY = -1;
                }
                else if (disX > disY)
                {
                    targetAngle += 90.0f;
                    disX = -1;
                    disY = -1;
                }

                if (targetAngle != 0)
                {
                    Rotate();
                }
            }*/
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