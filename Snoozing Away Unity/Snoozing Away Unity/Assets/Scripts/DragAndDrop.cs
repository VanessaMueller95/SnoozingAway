using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabPlacementObject;

    GameObject hitObject = null;
    Vector3 normal;

    private GameObject instancedItem;
    private GameObject parent;

    //ermöglicht es bestimmte Layer beim Casten zu ignorieren, kann im Inspector definiert werden
    public LayerMask mask;

    public bool stairs;

    //Handlung beim Start der Bewegung
    public void OnBeginDrag(PointerEventData eventData)
    {
        //deaktiviert die Swipe-Funktion der Kamera solange das Drag & Drop aktiv ist
        GameObject.Find("MainCamera").GetComponent<camera>().enabled = false;

        parent = GameObject.Find("UI");
        //Instanziiert das Hinweisbild
        instancedItem = Instantiate(gameObject, parent.transform);
        instancedItem.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    //Position wird während Bewegung geupdatet
    public void OnDrag(PointerEventData eventData)
    {
        instancedItem.transform.position = Input.mousePosition;
    }

    //Beim Loslassen
    public void OnEndDrag(PointerEventData eventData)
    {
        //Hinweisobjekt wird zerstört
        Destroy(instancedItem);

        //Camera Swipe wird wieder aktiviert
        GameObject.Find("MainCamera").GetComponent<camera>().enabled = true;

        //Plazierung des Wegfeldes an der Stelle an der Losgelassen wurde
        Vector3 point;

        if (getTargetLocation(out point))
        {
            if (!stairs)
            {
                //kann nur auf Wegfeldern plaziert werden (Code 0)
                if (hitObject.GetComponent<Cube>().code == 0)
                {
                    Vector3 placentPosition = hitObject.transform.position + normal;

                    //testet ob bereits ein Interaktionsfeld vorhanden ist und zerstört es falls ja
                    Collider[] hitColliders = Physics.OverlapSphere(placentPosition, (float)0.5);
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        Debug.Log(hitColliders[i].gameObject);
                        if (hitColliders[i].gameObject.tag == "Interact")
                        {
                            Destroy(hitColliders[i].gameObject);
                        }
                    }

                    //Plazierung abhängig von der Normalen der jeweiligen Seitenfläche 
                    var startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(90, 0, 0);
                    if (normal == new Vector3(0, -1, 0))
                    {
                        startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(90, 180, 0);
                    }
                    if (normal == new Vector3(0, 1, 0))
                    {
                        startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(90, -180, 0);
                    }
                    if (normal == new Vector3(0, 0, -1))
                    {
                        startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(-90, 180, 0);
                    }
                    if (normal == new Vector3(0, 0, 1))
                    {
                        startRot = Quaternion.LookRotation(normal) * Quaternion.Euler(-90, 0, 0);
                    }
                    if (normal == new Vector3(1, 0, 0))
                    {
                        startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(-90, 180, 0);
                    }
                    if (normal == new Vector3(-1, 0, 0))
                    {
                        startRot = Quaternion.LookRotation(-normal) * Quaternion.Euler(-90, -180, 0);
                    }
                    //Instanziierung des Wegfeldes
                    Instantiate(prefabPlacementObject, placentPosition, startRot);
                }
            }
            //Falls es sich um Treppen handelt können diese nur auf den Platzhaltern plaziert werden (Code 5)
            else if(stairs)
            {
                if (hitObject.GetComponent<Cube>().code == 5)
                {
                    Instantiate(prefabPlacementObject, hitObject.transform.position, hitObject.transform.rotation);
                }
            }
        }
    }

    //Raycasting
    bool getTargetLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
        {
                point = hitInfo.point;
                hitObject = hitInfo.collider.gameObject;
                normal = hitInfo.normal;
                return true;
        }
        //wenn der Mauszeiger oder Finger nicht auf der Fläche liegt wird false zurückgegeben 
        point = Vector3.zero;
        return false;
    }
}
