using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snoozer : MonoBehaviour
{
    Vector3 noAngle;
    Vector3 newVector;

    RaycastHit hitDown;
    RaycastHit hitFloor;
    RaycastHit hitWall;

    public LayerMask mask;

    public GameObject restartMenuUI;
    public Quaternion spreadAngle;

    private void Start()
    {
        spreadAngle = Quaternion.AngleAxis(10, new Vector3(0, 0, 1));
    }

    void Update()
    {
        Debug.Log(Time.timeScale);
        
        noAngle = transform.TransformDirection(Vector3.down);
        newVector = spreadAngle * noAngle;

        Debug.Log(Time.timeScale);

        //Character Bewegung nach Vorne
        transform.Translate(Vector3.forward * Time.deltaTime);

        //Raycast Zeichnung zur Visualisierung
        Debug.DrawRay(transform.position, newVector * 10f, Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1f, Color.blue);

        //Positionierung von Snoozer auf dem Boden mit einem Raycast nach Unten
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 10f, out hitFloor, Mathf.Infinity, mask))
        {
            //Berechnung des Abstandes vom Boden
            Vector3 distance = hitFloor.point + transform.up * 0.5f;
            //Animation der Positionsänderung
            transform.position = Vector3.Lerp(transform.position, distance, Time.deltaTime * 2);
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
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 3);
            }
        }

        //Raycast nach unten um zu Testen ob es einen Boden gibt und Positionierung an Boden-Normale
        if (Physics.Raycast(transform.position, newVector * 10f, out hitDown, Mathf.Infinity, mask))
        {
            //Test ob Bodennormale Charakter Normalen entspricht
            if (transform.up != hitDown.normal)
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hitDown.normal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 3);
            }
        }

    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision");
        if (col.gameObject.tag == "water")
        {
            Debug.Log("Water");
            //Destroy(transform.gameObject);
            spreadAngle = Quaternion.AngleAxis(10, new Vector3(0, 0, 1));
            restartMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }

        if (col.gameObject.tag == "ziel")
        {
            Debug.Log("Ziel");
            //Destroy(transform.gameObject);
            spreadAngle = Quaternion.AngleAxis(10, new Vector3(0, 0, 1));
            restartMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }

        if (col.gameObject.tag == "eule")
        {
            Debug.Log("Eule");
            Destroy(col.gameObject);
            GameObject.Find("TimerCanvas").GetComponent<Timer>().targetTime += (float)5.0;
        }

        if (col.gameObject.tag == "rabe")
        {
            Debug.Log("Rabe");
            Destroy(col.gameObject);
            GameObject.Find("TimerCanvas").GetComponent<Timer>().targetTime -= (float)5.0;
        }
    }
}

        
        
