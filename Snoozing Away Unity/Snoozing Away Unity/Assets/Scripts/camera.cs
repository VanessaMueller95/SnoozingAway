using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    private Vector3[] cameraPoints;
    private int currentCameraPos = 2;

    //Variablen Swipe Controls
    private Vector3 fp;   //Touch Anfangsposition
    private Vector3 lp;   //Touch Endposition
    private float dragDistance;  //minimale Laenge um Swipe auszulösen

    public float zoom;

    // Start is called before the first frame update
    void Start()
    {

        var cuboid = GameObject.Find("Cuboid");
        var cuboidSkript = GameObject.Find("Cuboid").GetComponent<Cuboid>();
        var dimensions = cuboidSkript.dimensions;
        var cellSize = cuboidSkript.cellSize;
        //var zoom = 1.6f;

        dragDistance = Screen.width * 10 / 100;

        var camDist = new Vector3(dimensions.x * cellSize * zoom,
            dimensions.x * cellSize * zoom,
            dimensions.x * cellSize * zoom);

        cameraPoints = new Vector3[6];
        cameraPoints[0] = Vector3.Scale(Vector3.forward, camDist);
        cameraPoints[1] = Vector3.Scale(Vector3.right, camDist);
        cameraPoints[2] = Vector3.Scale(Vector3.back, camDist);
        cameraPoints[3] = Vector3.Scale(Vector3.left, camDist);
        cameraPoints[4] = Vector3.Scale(Vector3.up, camDist);
        cameraPoints[5] = Vector3.Scale(Vector3.down, camDist);
    }

    // Update is called once per frame
    void Update()
    {
        
            // get the target position
            var target = cameraPoints[currentCameraPos];
            var target2 = cameraPoints[0];


            // transform update only if we haven't reached the point
            var delta = transform.position - target;

            // never compare on 0 with FP ;) - remember PROG1 
            // this should be Mathf.Epsilon: thanks Unity for breaking this convention
            if (delta.sqrMagnitude > 0.1F)
            {
                //Debug.Log(delta.sqrMagnitude);

                // can make the 'whoopiness' adjustable with last parameter of slerp
                transform.position = Vector3.Slerp(transform.position, target, 0.4f);
                if (currentCameraPos == 4)
                {
                    transform.LookAt(Vector3.zero, Vector3.forward);
                }
                else if (currentCameraPos == 5)
                {
                    transform.LookAt(Vector3.zero, Vector3.back);
                }
                else
                {
                    transform.LookAt(Vector3.zero, Vector3.up);
                }


            }

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
                                if (currentCameraPos == 4 || currentCameraPos == 5)
                                {
                                    currentCameraPos = 2;
                                }
                                else
                                {
                                    currentCameraPos++;
                                    if (currentCameraPos > 3)
                                    {
                                        currentCameraPos = 0;
                                    }
                                }

                            }
                            else //Left swipe
                            {
                                if (currentCameraPos == 4 || currentCameraPos == 5)
                                {
                                    currentCameraPos = 2;
                                }
                                else
                                {
                                    currentCameraPos--;
                                    if (currentCameraPos < 0)
                                    {
                                        currentCameraPos = 3;
                                    }
                                }
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                if (currentCameraPos == 4)
                                {
                                    currentCameraPos = 2;
                                }
                                else
                                {
                                    currentCameraPos = 5;
                                }
                            }
                            else
                            {   //Down swipe
                                if (currentCameraPos == 5)
                                {
                                    currentCameraPos = 2;
                                }
                                else
                                {
                                    currentCameraPos = 4;
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentCameraPos = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentCameraPos = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentCameraPos = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentCameraPos = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                currentCameraPos = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                currentCameraPos = 5;
            }
        }

    
}
