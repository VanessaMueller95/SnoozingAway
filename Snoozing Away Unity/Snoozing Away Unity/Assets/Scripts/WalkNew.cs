﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkNew : MonoBehaviour
{

    private Cuboid cuboid;

    private int currentPos;
    private int currentSide = 0;

    private bool inAnimation = false;

    private Vector3[] sideDirections = {
        Vector3.up,Vector3.left,Vector3.right,Vector3.down,Vector3.forward,Vector3.back
    };

    RaycastHit hitDown; RaycastHit hitFloor; RaycastHit hitWall;

    private float walkingSpeed = 2;

    public string lastRotation = null;

    public LayerMask mask;

    bool blinkEnd = false;
    public Animator animator;
    public AudioManager audiomanager;

    public GameObject restartMenuUI;
    public GameObject wonMenuUI;
    public Quaternion spreadAngle;


    // Start is called before the first frame update
    void Start()
    {
        cuboid = FindObjectOfType<Cuboid>();
        animator = transform.Find("Snoozer-Walking").GetComponent<Animator>();

        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        yield return new WaitUntil(() => cuboid.levelLoaded = true);

        Reset();
        Vector3 sideVector = sideDirections[currentSide];

        // initial position from this point out we should cast the rays 
        Vector3 worldPos = (Vector3)Cuboid.GetPosition(currentPos, cuboid.Dimensions) * cuboid.cellSize - cuboid.CenterPoint;
        gameObject.transform.localPosition = worldPos + sideVector * cuboid.cellSize;

        StartCoroutine(Blink(3, "start"));

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.up);
        if (blinkEnd == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * walkingSpeed);
        }

        //Raycast Debug
        Debug.DrawRay(transform.position + transform.up, (Quaternion.AngleAxis(-85, transform.right) * (transform.transform.forward * -1))*5, Color.yellow);
        Debug.DrawRay(transform.position + transform.up, transform.TransformDirection(Vector3.forward) * 10f, Color.blue);

        if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(Vector3.down) * 1f, out hitFloor, Mathf.Infinity, mask))
        {
            Vector3 distance = hitFloor.point + transform.up * 0.1f;
            //Debug.Log("Boden getroffen, Abstand: " + hitDown.distance);
            //Debug.Log("Walking on: " + hitFloor.collider.gameObject.tag);
            //Berechnung des Abstandes vom Boden

            //Debug.Log("Walking on: " + hitFloor.collider.gameObject.GetComponent<Cube>().code);
            //Debug.Log("0: Boden; 1: Wasser; 2: Eule; 3: Raben; 4:Treppe");
            if (hitDown.distance < 2)
            {
                transform.position = Vector3.Lerp(transform.position, distance, Time.deltaTime * (walkingSpeed * 4));
            }
            if (hitFloor.collider.gameObject.GetComponent<Cube>().code == 1)
            {
                Debug.Log("Water");
                blinkEnd = false;
                animator.enabled = false;
                StartCoroutine(Blink(2, "water"));
            }
        }
        
        if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(Vector3.forward) * 1f, out hitWall, 1, mask))
        {
            if (hitWall.collider.gameObject.tag != "stairs" && hitWall.collider.gameObject.tag != "turnAround" && hitWall.distance < 1.5)
            {
                //Debug.Log("Tag Wand vorne, in Methode : " + hitWall.collider.gameObject.tag);
                //Rotation von Snoozer in Richtung der Normalen der Wand
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hitWall.normal) * transform.rotation;
                //Animation der Position von Snoozer
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * (walkingSpeed*4));
            }
            else if (hitWall.collider.gameObject.tag == "turnAround" && hitWall.distance < 0.3)
            {
                Debug.Log(transform.up);
                Debug.Log(transform);
                if (V3Equal(transform.up, Vector3.back))
                {
                    Debug.Log("TEST");
                    transform.position = transform.position + (transform.forward * (float)2 + (-transform.right * (float)2));
                }
                else if (V3Equal(transform.up, Vector3.forward))
                {
                    transform.position = transform.position + (transform.forward * (float)2 + (transform.right * (float)2));
                }
                else if (V3Equal(transform.up, Vector3.left))
                {
                    transform.position = transform.position + (transform.forward * (float)2 + (transform.right * (float)2));

                }
                else if (V3Equal(transform.up, Vector3.right))
                {
                    transform.position = transform.position + (transform.forward * (float)2 + (-transform.right * (float)2));
                }
                //transform.rotation = Quaternion.LookRotation(hitWall.normal, transform.up);
            }
        }
        
        
        //Raycast nach unten um zu Testen ob es einen Boden gibt und Positionierung an Boden-Normale
        if (Physics.Raycast(transform.position, (Quaternion.AngleAxis(-82, transform.right) * (transform.transform.forward * -1)) * 10f, out hitDown, Mathf.Infinity, mask))
        {
            //Test ob Bodennormale Charakter Normalen entspricht
            if ( transform.up != hitDown.normal)
            {
                Quaternion newRotation = transform.rotation;
                newRotation = Quaternion.FromToRotation(transform.up, hitDown.normal) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * (float)8);
            }
        }
        
    }

    public void Reset()
    {
        Debug.Log("enabled: " + cuboid.GetEnabledCount);

        for (var i = 0; i < cuboid.CellCount; i++)
        {
            // the snoozer can be only attached to enabled cells 
            // needs to be revised ...
            if (cuboid.Cells[i].enabled == true)
            {
                Debug.Log("found cell:" + i);

                // need to check walkable or not 

                currentPos = i;

                return; // stop here
            }
        }
    }

    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    IEnumerator Blink(float waitTime, string state)
    {
        //Berechnung der Endzeit
        var endTime = Time.time + waitTime;

        //Holt sich alle Render-Objekte von dem Charakter
        Renderer[] rs = GetComponentsInChildren<Renderer>();

        //Schaltet die Renderer regelmäßig an und aus -> Blinken
        while (Time.time < endTime)
        {
            Debug.Log("Aktiv");
            foreach (Renderer r in rs) { r.enabled = false; }
            yield return new WaitForSeconds(0.2f);
            Debug.Log("Aktiv");
            foreach (Renderer r in rs) { r.enabled = true; }
            yield return new WaitForSeconds(0.2f);
        }

        blinkEnd = true;
        animator.enabled = true;

        //Anhängig von dem Grund für das Blinken werden Folgeaktionen ausgeführt am Ende:
        //Bei Start wird das Ticken aktiviert und der Timer gestartet
        /*if (state == "start")
        {
            GameObject.Find("TimerCanvas").GetComponent<Timer>().timerActive = true;
            audiomanager.Play("Ticking");
        }*/

        //Bei Wasser wird das Restart Menü aufgerufen, die Zeit angehalten und das Ticken beendet
        if (state == "water")
        {
            restartMenuUI.SetActive(true);
            Time.timeScale = 0f;
            //audiomanager.Stop("Ticking");
        }
        /*
        //Im Ziel wird das Gewonnen Menü aufgerufen, die Zeit angehalten und das Ticken beendet
        if (state == "ziel")
        {
            wonMenuUI.SetActive(true);
            Time.timeScale = 0f;
            audiomanager.Stop("Ticking");
        }*/
    }
}
