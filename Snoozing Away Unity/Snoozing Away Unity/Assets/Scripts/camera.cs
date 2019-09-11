using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    private Vector3[] cameraPoints;
    private int currentCameraPos = 1;

    //Variablen Swipe Controls
    private Vector3 fp;   //Touch Anfangsposition
    private Vector3 lp;   //Touch Endposition
    private float dragDistance;  //minimale Laenge um Swipe auszulösen

    // Start is called before the first frame update
    void Start()
    {
        var dimensions = GameObject.Find("Cuboid").GetComponent<Cuboid>().dimensions;
        var cellSize = GameObject.Find("Cuboid").GetComponent<Cuboid>().cellSize;
        var zoom = 1.4f;

        dragDistance = Screen.width * 10 / 100;

        var camDist = new Vector3(dimensions.x * cellSize * zoom,
            dimensions.x * cellSize * zoom,
            dimensions.x * cellSize * zoom);

        cameraPoints = new Vector3[6];
        cameraPoints[0] = Vector3.Scale(Vector3.forward, camDist);
        cameraPoints[1] = Vector3.Scale(Vector3.back, camDist);
        cameraPoints[2] = Vector3.Scale(Vector3.left, camDist);
        cameraPoints[3] = Vector3.Scale(Vector3.right, camDist);
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
        var delta = Camera.main.transform.position - target;

        // never compare on 0 with FP ;) - remember PROG1 
        // this should be Mathf.Epsilon: thanks Unity for breaking this convention
        if (delta.sqrMagnitude > 0.1F)
        {

            // can make the 'whoopiness' adjustable with last parameter of slerp
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, target, 0.4f);
            if (currentCameraPos == 4)
            {
                Camera.main.transform.LookAt(Vector3.zero, Vector3.forward);
            }
            else if (currentCameraPos == 5)
            {
                Camera.main.transform.LookAt(Vector3.zero, Vector3.back);
            }
            else
            {
                Camera.main.transform.LookAt(Vector3.zero, Vector3.up);
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
                            currentCameraPos++;
                            if (currentCameraPos > 3)
                            {
                                currentCameraPos = 0;
                            }

                        }
                        else //Left swipe
                        {
                            currentCameraPos--;
                            if (currentCameraPos < 0)
                            {
                                currentCameraPos = 3;
                            }
                        }
                    }
                }
            }
        }
    }
}
