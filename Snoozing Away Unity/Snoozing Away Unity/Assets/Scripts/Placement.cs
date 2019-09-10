using UnityEngine;

public class Placement : MonoBehaviour
{

    //TODO PLACEMENT SKRIPT SOLLTE TREPPEN COLLIDER IGNORIEREN
    public GameObject prefabPlacementObject;
    public GameObject dragObject;

    bool mouseClick = false;
    GameObject hitObject = null;
    Vector3 normal;

    private GameObject instancedItem;
    private GameObject parent;
    Vector3 startPosition;
    
    void Update()
    {
        Vector3 point;

        if (getTargetLocation(out point))
        {
            Vector3 placentPosition = hitObject.transform.position + normal;

            var startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(0, 0, 0);

            if (Input.GetMouseButtonDown(0) && mouseClick == false)
            {
                Debug.Log("Testi");
                mouseClick = true;
                if(normal== new Vector3(0, -1, 0))
                {
                    Debug.Log("TEEEEEEEEEEEEEEEEEEEEEST");
                    startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(90, 180, 0);
                }
                if (normal == new Vector3(0, 1, 0))
                {
                    Debug.Log("TEEEEEEEEEEEEEEEEEEEEEST");
                    startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(90, -180, 0);
                }
                if (normal == new Vector3(0, 0, -1))
                {
                    Debug.Log("TEEEEEEEEEEEEEEEEEEEEEST");
                    startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(-90, 0, 0);
                }
                if (normal == new Vector3(0, 0, 1))
                {
                    Debug.Log("TEEEEEEEEEEEEEEEEEEEEEST");
                    startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(90, 0, 0);
                }
                if (normal == new Vector3(1, 0, 0))
                {
                    Debug.Log("TEEEEEEEEEEEEEEEEEEEEEST");
                    startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(90, 180, 0);
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
            if (hitInfo.collider.gameObject.GetComponent<Cube>().code == 0)
            {
                point = hitInfo.point;
                //Debug.Log("Punkt: " + point);
                hitObject = hitInfo.collider.gameObject;
                //Debug.Log("GameObject: " + hitObject);
                //Debug.Log("Tag: " + hitObject.tag);
                normal = hitInfo.normal;
                //Debug.Log("Normal: " + normal);
                return true;
            }
        }
        //wenn der Mauszeiger nicht auf der Fläche liegt wird false zurückgegeben 
        point = Vector3.zero;
        return false;
    }

    public void changePlacementObject(GameObject place)
    {
        prefabPlacementObject = place;
    }
}
