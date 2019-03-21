using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snoozer : MonoBehaviour
{
    //Variablen zur Winkle-Berechnung des Abwärts-Raycasts 
    Vector3 noAngle;
    Vector3 newVector;

    //Hit Variablen für die Raycasts
    RaycastHit hitDown;
    RaycastHit hitFloor;
    RaycastHit hitWall;

    //LayerMaske, die es Raycasts ermöglicht bestimmte Ebenen zu ignorieren, kann über den Editor eingestellt werden
    public LayerMask mask;

    //Variablen für die 3 Menüarten, müssen über den Editor gesetzt werden
    public GameObject restartMenuUI;
    public GameObject wonMenuUI;
    public Quaternion spreadAngle;

    bool blinkEnd = false;
    public Animator animator;
    public AudioManager audiomanager;

    private void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        StartCoroutine(Blink(3, "start"));
        spreadAngle = Quaternion.AngleAxis(4, new Vector3(0, 0, 1));
    }

    void Update()
    {
        if (blinkEnd == true)
        {
            Walk();
        }

        //Raycast Zeichnung zur Visualisierung/Debugging
        Debug.DrawRay(transform.position, newVector * 10f, Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1f, Color.blue);

        //Positionierung von Snoozer auf dem Boden mit einem Raycast nach Unten
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 10f, out hitFloor, Mathf.Infinity, mask))
        {
            //Berechnung des Abstandes vom Boden
            Vector3 distance = hitFloor.point + transform.up * 0.45f;
            //Animation der Positionsänderung
            transform.position = Vector3.Lerp(transform.position, distance, Time.deltaTime * 4);
        }

        //Raycast nach vorne um Wände zu erkennen 
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * 1f, out hitWall, 1, mask))
        {
            //Test auf Entfernung zur Wand
            if (hitWall.distance < 0.5)
            {
                //Rotation von Snoozer in Richtung der Normalen der Wand
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hitWall.normal) * transform.rotation;
                //Animation der Position von Snoozer
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 5);
            }
        }

        //Berechnung des Winkels für den Raycast nach Unten, damit 90Grad Kanten erkannt werden
        noAngle = transform.TransformDirection(Vector3.down);
        newVector = spreadAngle * noAngle;

        //Raycast nach unten um zu Testen ob es einen Boden gibt und Positionierung an Boden-Normale
        if (Physics.Raycast(transform.position, newVector * 10f, out hitDown, Mathf.Infinity, mask))
        {
            //Test ob Bodennormale Charakter Normalen entspricht
            if (transform.up != hitDown.normal)
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hitDown.normal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 4);
            }
        }

    }

    public void Walk()
    {
        //Character Bewegung nach Vorne
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    //Verhalten bei Kollisionen
    void OnCollisionEnter(Collision col)
    {
        //Kollision mit Wasserflächen, Gameover Screen 
        if (col.gameObject.tag == "water")
        {
            Debug.Log("Water");
            blinkEnd = false;
            animator.enabled = false;
            StartCoroutine(Blink(2, "water"));
            audiomanager.Play("Water");
        }

        //Kollision mit dem Ziel, Gewonnen Screen
        if (col.gameObject.tag == "ziel")
        {
            Debug.Log("Ziel");
            blinkEnd = false;
            animator.enabled = false;
            StartCoroutine(Blink(2, "ziel"));

        }

        //Kollision mit Eulen, Zeit wird um 5 Sekunden verlängert
        if (col.gameObject.tag == "eule")
        {
            Debug.Log("Eule");
            Destroy(col.gameObject);
            GameObject.Find("TimerCanvas").GetComponent<Timer>().targetTime += (float)5.0;
            audiomanager.Play("Owl");
        }

        //Kollision mit Raben, Zeit wird um 5 Sekunden reduziert
        if (col.gameObject.tag == "rabe")
        {
            Debug.Log("Rabe");
            Destroy(col.gameObject);
            GameObject.Find("TimerCanvas").GetComponent<Timer>().targetTime -= (float)5.0;
            audiomanager.Play("Raven");
        }
    }

    IEnumerator Blink(float waitTime, string state)
    {
        Debug.Log("In Funktion");
        var endTime = Time.time + waitTime;

        Renderer[] rs = GetComponentsInChildren<Renderer>();

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

        if (state == "start")
        {
            GameObject.Find("TimerCanvas").GetComponent<Timer>().timerActive = true;
            audiomanager.Play("Ticking");
        }

        if (state == "water")
        {
            restartMenuUI.SetActive(true);
            Time.timeScale = 0f;
            audiomanager.Stop("Ticking");
        }

        if (state == "ziel")
        {
            wonMenuUI.SetActive(true);
            Time.timeScale = 0f;
            audiomanager.Stop("Ticking");
        }
    }
}

        
        
