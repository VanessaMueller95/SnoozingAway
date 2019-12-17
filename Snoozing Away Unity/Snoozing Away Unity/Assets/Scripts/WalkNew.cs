using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkNew : MonoBehaviour
{
    //Variablen zur Verwaltung von Snoozer
    private Cuboid cuboid;

    private int currentPos;
    private int currentSide = 0;

    private Vector3[] sideDirections = {
        Vector3.up,Vector3.left,Vector3.right,Vector3.down,Vector3.forward,Vector3.back
    };

    private float walkingSpeed = 2;
    public string lastRotation = null;

    //Layermaske, die von den Raycasts von Snoozer ignoriert werden 
    public LayerMask mask;
    RaycastHit hitDown; RaycastHit hitFloor; RaycastHit hitWall;

    //Animationsvariablen und Audio
    bool blinkEnd = false;
    public Animator animator;
    public AudioManager audiomanager;

    //Menüs
    public GameObject restartMenuUI;
    public GameObject wonMenuUI;
    public Quaternion spreadAngle;

    //Wecker
    public GameObject clock;
    private int clockPosition;

    bool water = false;

    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        Time.timeScale = 1f;
        cuboid = FindObjectOfType<Cuboid>();
        animator = transform.Find("Snoozer-Walking").GetComponent<Animator>();
        clock = GameObject.Find("alarmclock");

        //Level wird gestartet
        StartCoroutine(StartLevel());
    }

    //Methode, die beim Start jedes Levels abläuft
    IEnumerator StartLevel()
    {
        //Level startet erst, wenn die Leveldatei vollständig geladen ist
        yield return new WaitUntil(() => cuboid.levelLoaded = true);

        Reset(); //positioniert Snoozer
        SetClock(); //positioniert den Wecker

        //wird zur korrekten Ausrichtung benötigt
        Vector3 sideVector = sideDirections[currentSide];

        // Position von Snoozer
        Vector3 worldPos = (Vector3)Cuboid.GetPosition(currentPos, cuboid.Dimensions) * cuboid.cellSize - cuboid.CenterPoint;
        gameObject.transform.localPosition = worldPos + sideVector * cuboid.cellSize;

        //Position vom Wecker
        Vector3 worldPosClock = (Vector3)Cuboid.GetPosition(clockPosition, cuboid.Dimensions) * cuboid.cellSize - cuboid.CenterPoint;
        clock.transform.localPosition = worldPosClock + Vector3.up * cuboid.cellSize;

        StartCoroutine(Blink(3, "start"));

    }

    // Update is called once per frame
    void Update()
    {
        //wenn die Animation beendet ist läuft Snoozer los
        if (blinkEnd == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * walkingSpeed);
        }

        //Raycast Debug
        Debug.DrawRay(transform.position + transform.up, transform.TransformDirection(Vector3.down) * 1f, Color.yellow);
        Debug.DrawRay(transform.position + transform.up, transform.TransformDirection(Vector3.forward) * 10f, Color.blue);

        //Raycast nach unten
        if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(Vector3.down) * 1f, out hitFloor, Mathf.Infinity, mask))
        {
            //Berechnung der Distanz und Ausrichtung am Boden
            Vector3 distance = hitFloor.point + transform.up * 0.1f;
            if (hitFloor.distance < 2)
            {
                transform.position = Vector3.Lerp(transform.position, distance, Time.deltaTime * (walkingSpeed * 4));
            }
            //Test auf Wasser (Code 1)
            if (hitFloor.distance < 2 && hitFloor.collider.gameObject.GetComponent<Cube>().code == 1)
            {
                //Level wird beendet
                if (!water)
                {
                    audiomanager.Play("Water");
                    blinkEnd = false;
                    animator.enabled = false;
                    StartCoroutine(Blink(2, "water"));
                    water = true;
                }
            }
        }
        
        //Raycast nach vorne
        if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(Vector3.forward) * 1f, out hitWall, 1, mask))
        {
            if (hitWall.collider.gameObject.tag != "turnAround" && hitWall.distance < 1.5)
            {
                //Rotation von Snoozer in Richtung der Normalen der Wand
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hitWall.normal) * transform.rotation;
                //Animation der Position von Snoozer
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * (walkingSpeed*5));
            }
            else if (hitWall.collider.gameObject.tag == "turnAround" && hitWall.distance < 0.3)
            {
                Debug.Log("TREPPE");
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
            }
        }
        
        //Raycast schräg nach unten um zu Testen ob es einen Boden gibt und Positionierung an Boden-Normale
        if (Physics.Raycast(transform.position, (Quaternion.AngleAxis(-82, transform.right) * (transform.transform.forward * -1)) * 10f, out hitDown, Mathf.Infinity, mask))
        {
            //Test ob Bodennormale Charakter Normalen entspricht
            if ( transform.up != hitDown.normal)
            {
                Quaternion newRotation = transform.rotation;
                newRotation = Quaternion.FromToRotation(transform.up, hitDown.normal) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * (float)12);
            }
        }
        
    }

    //Position Snoozers auf Start-Plattform (Code 6)
    public void Reset()
    {

        for (var i = 0; i < cuboid.CellCount; i++)
        {
            if (cuboid.Cells[i].code == 6)
            {
                Debug.Log("found cell:" + i);

                currentPos = i;

                return; 
            }
        }
    }

    //Position Weckers auf End-Plattform (Code 7)
    public void SetClock()
    {

        for (var i = 0; i < cuboid.CellCount; i++)
        {
            if (cuboid.Cells[i].code == 7)
            {
                Debug.Log("found cell:" + i);

                clockPosition = i;

                return;
            }
        }
    }

    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    //Blinkanimtion, Parameter: Dauer und Auslöser
    IEnumerator Blink(float waitTime, string state)
    {
        Debug.Log("STATE: " + state);
        //Berechnung der Endzeit
        var endTime = Time.time + waitTime;

        //Holt sich alle Render-Objekte von dem Charakter
        Renderer[] rs = GetComponentsInChildren<Renderer>();

        //Schaltet die Renderer regelmäßig an und aus -> Blinken
        while (Time.time < endTime)
        {
            foreach (Renderer r in rs) { r.enabled = false; }
            yield return new WaitForSeconds(0.2f);
            foreach (Renderer r in rs) { r.enabled = true; }
            yield return new WaitForSeconds(0.2f);
        }

        blinkEnd = true;
        animator.enabled = true;

        //Anhängig von dem Grund für das Blinken werden Folgeaktionen ausgeführt am Ende:
        //Bei Start wird das Ticken aktiviert und der Timer gestartet
        if (state == "start")
        {
            GameObject.Find("TimerImage").GetComponent<ImageTimer>().timerActive = true;
            audiomanager.Play("Ticking");
        }

        //Bei Wasser wird das Restart Menü aufgerufen, die Zeit angehalten und das Ticken beendet
        if (state == "water")
        {
            restartMenuUI.SetActive(true);
            Time.timeScale = 0f;
            audiomanager.Stop("Ticking");
        }
    }

    //Blinkanimation des Weckers
    IEnumerator ClockBlink(float waitTime)
    {
        var endTime = Time.time + waitTime;

        //Holt sich alle Render-Objekte von dem Charakter
        Renderer[] rs = clock.GetComponentsInChildren<Renderer>();

        //Schaltet die Renderer regelmäßig an und aus -> Blinken
        while (Time.time < endTime)
        {
            foreach (Renderer r in rs) { r.enabled = false; }
            yield return new WaitForSeconds(0.2f);
            foreach (Renderer r in rs) { r.enabled = true; }
            yield return new WaitForSeconds(0.2f);
        }

        blinkEnd = true;
        animator.enabled = true;

        wonMenuUI.SetActive(true);
        Time.timeScale = 0f;
        audiomanager.Stop("Ticking");
    }

    //Collision Events von Snoozer
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ziel")
        {
            Debug.Log("Ziel");
            blinkEnd = false;
            animator.enabled = false;
            StartCoroutine(ClockBlink(2));
        }

        if (other.gameObject.tag == "eule")
        {
            Debug.Log("Eule");
            Destroy(other.gameObject);
            GameObject.Find("TimerImage").GetComponent<ImageTimer>().time += (float)5.0;
            audiomanager.Play("Owl");
        }

        //Kollision mit Raben, Zeit wird um 5 Sekunden reduziert
        if (other.gameObject.tag == "rabe")
        {
            Debug.Log("Rabe");
            Destroy(other.gameObject);
            GameObject.Find("TimerImage").GetComponent<ImageTimer>().time -= (float)5.0;
            audiomanager.Play("Raven");
        }
    }
}
