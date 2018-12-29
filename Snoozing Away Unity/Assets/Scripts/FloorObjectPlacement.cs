using UnityEngine;
using System.Collections;
using System;

//Script für die Plazierung von Wegfeldern 
public class FloorObjectPlacement : MonoBehaviour
{
    //Prefab, das positioniert werden soll
    public GameObject prefabPlacementObject;

    //Prefab für Hinweis auf Plazierbarkeit
    public GameObject prefabOK;

    //Prefab für Hinweis auf keine Plazierbarkeit 
    public GameObject prefabFail;

    //Gittergröße
    public float grid = 2.0f;

    // Speichert welche Felder bereits belegt sind
    int[,] usedSpace;
    
    //Objekt, das plaziert wird
    GameObject placementObject = null;
    //Objekt, das Verfügbarkeit zeigt 
    GameObject areaObject = null;

    bool mouseClick = false;
    Vector3 lastPos;

    // Berechnung des Gitters und Initialisierung des Platz Arrays 
    void Start()
    {
        Vector3 slots = GetComponent<Renderer>().bounds.size / grid;
        usedSpace = new int[Mathf.CeilToInt(slots.x), Mathf.CeilToInt(slots.z)];
        for (var x = 0; x < Mathf.CeilToInt(slots.x); x++)
        {
            for (var z = 0; z < Mathf.CeilToInt(slots.z); z++)
            {
                usedSpace[x, z] = 0;
            }
        }
    }

    void Update()
    {
        Vector3 point;

        // Test auf Maus-Ray Collision
        if (getTargetLocation(out point))
        {
            Vector3 halfSlots = GetComponent<Renderer>().bounds.size / 2.0f;

            //Berechnung der Positionen
            int x = (int)Math.Round(Math.Round(point.x - transform.position.x + halfSlots.x - grid / 2.0f) / grid);
            int z = (int)Math.Round(Math.Round(point.z - transform.position.z + halfSlots.z - grid / 2.0f) / grid);

            //Berechnung der Weltkooordinaten basierend auf dem Gitter 
            point.x = (float)(x) * grid - halfSlots.x + transform.position.x + grid / 2.0f;
            point.z = (float)(z) * grid - halfSlots.z + transform.position.z + grid / 2.0f;

            //Erzeugung eines Objekts, dass die Verfügbarkeit der Fläche darstellt
            if (lastPos.x != x || lastPos.z != z || areaObject == null)
            {
                lastPos.x = x;
                lastPos.z = z;
                if (areaObject != null)
                {
                    Destroy(areaObject);
                }
                areaObject = (GameObject)Instantiate(usedSpace[x, z] == 0 ? prefabOK : prefabFail, point, Quaternion.identity);
            }

            // Objekt wird initialisiert oder bewegt 
            if (!placementObject)
            {
                placementObject = (GameObject)Instantiate(prefabPlacementObject, point, Quaternion.identity);
            }
            else
            {
                placementObject.transform.position = point;
            }

            // Bei Linksklick wird das Objekt in die Szene eingefügt und die Position als besetzt makiert
            if (Input.GetMouseButtonDown(0) && mouseClick == false)
            {
                mouseClick = true;

                if (usedSpace[x, z] == 0)
                {
                    Debug.Log("Placement Position: " + x + ", " + z);
                    usedSpace[x, z] = 1;
                    Instantiate(prefabPlacementObject, point, Quaternion.identity);
                }
            }
            else if (!Input.GetMouseButtonDown(0))
            {
                mouseClick = false;
            }

        }
        //Wenn die Maus nicht mehr auf der Fläche liegt wird das Objekt zerstört 
        else
        {
            if (placementObject)
            {
                Destroy(placementObject);
                placementObject = null;
            }
            if (areaObject)
            {
                Destroy(areaObject);
                areaObject = null;
            }
        }
    }

    //Ziel des Mauszeigers wird zurückgegeben 
    bool getTargetLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider == GetComponent<Collider>())
            {
                point = hitInfo.point;
                return true;
            }
        }
        //wenn der Mauszeiger nicht auf der Fläche liegt wird false zurückgegeben 
        point = Vector3.zero;
        return false;
    }
}