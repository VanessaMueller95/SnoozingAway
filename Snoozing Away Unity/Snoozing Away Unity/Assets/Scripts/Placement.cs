using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject prefabPlacementObject;

    bool mouseClick = false;
    GameObject hitObject = null;
    Vector3 normal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point;

        if (getTargetLocation(out point))
        {
            Vector3 placentPosition = hitObject.transform.position + normal;
            Debug.Log("ObjectPosition: " + hitObject.transform.position);
            Debug.Log("PlacementPosition: " + placentPosition);

            if (Input.GetMouseButtonDown(0) && mouseClick == false)
            {
                mouseClick = true;
                var startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(90, 0, 0);
                if(normal== new Vector3(0, -1, 0))
                {
                    startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(90, 180, 0);
                }
                if (normal == new Vector3(0, 1, 0))
                {
                    startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(90, -180, 0);
                }
                Debug.Log("Rotation: " + startRot);
                Instantiate(prefabPlacementObject, placentPosition, startRot);
                
            }
            else if (!Input.GetMouseButtonDown(0))
            {
                mouseClick = false;
            }
        }
    }

    bool getTargetLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.gameObject.tag == "floor")
            {
                point = hitInfo.point;
                Debug.Log("Punkt: " + point);
                hitObject = hitInfo.collider.gameObject;
                Debug.Log("GameObject: " + hitObject);
                Debug.Log("Tag: " + hitObject.tag);
                normal = hitInfo.normal;
                Debug.Log("Normal: " + normal);
                return true;
            }
        }
        //wenn der Mauszeiger nicht auf der Fläche liegt wird false zurückgegeben 
        point = Vector3.zero;
        return false;
    }
}
