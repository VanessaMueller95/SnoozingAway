using System.Collections;
using UnityEngine;

public class Snoozer : MonoBehaviour
{
    //Variablen zur Winkle-Berechnung des Abwärts-Raycasts 
    Vector3 noAngle;
    Vector3 newVector;

    //Hit Variablen für die Raycasts
    RaycastHit hitDown;
    RaycastHit hitFloor;
    RaycastHit hitWall;

    //LayerMaske, die es Raycasts ermöglicht bestimmte Ebenen zu ignorieren, kann über den Inspektor eingestellt werden
    public LayerMask mask;

    //Variablen für die 3 Menüarten, können über den Inspektor gesetzt werden
    public GameObject restartMenuUI;
    public GameObject wonMenuUI;
    public Quaternion spreadAngle;

    //Variablen für die Animation und den Audiomanager
    bool blinkEnd = false;
    public Animator animator;
    public AudioManager audiomanager;



    private void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();

        //3 Sekunden Blinken des Charakters bevor die Zeit startet
        StartCoroutine(Blink(3, "start"));

        //Festlegen des Winkels für den Abwärts-Raycast
        spreadAngle = Quaternion.AngleAxis(4, new Vector3(0, 0, 1));
    }

    void Update()
    {
        Debug.Log(transform.forward);
        //Snoozer läuft gerade au, wenn er nicht Binkt
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

    //Funktion, die den Charakter beim Start, Ende oder bei bestimmten Kollisionen blinken lässt
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
        if (state == "start")
        {
            GameObject.Find("TimerCanvas").GetComponent<Timer>().timerActive = true;
            audiomanager.Play("Ticking");
        }

        //Bei Wasser wird das Restart Menü aufgerufen, die Zeit angehalten und das Ticken beendet
        if (state == "water")
        {
            restartMenuUI.SetActive(true);
            Time.timeScale = 0f;
            audiomanager.Stop("Ticking");
        }

        //Im Ziel wird das Gewonnen Menü aufgerufen, die Zeit angehalten und das Ticken beendet
        if (state == "ziel")
        {
            wonMenuUI.SetActive(true);
            Time.timeScale = 0f;
            audiomanager.Stop("Ticking");
        }
    }
}

        
        
