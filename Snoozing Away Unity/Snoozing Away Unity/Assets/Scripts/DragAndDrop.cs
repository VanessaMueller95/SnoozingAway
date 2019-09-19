using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabPlacementObject;

    GameObject hitObject = null;
    Collider hitCollider = null;
    Vector3 normal;

    private GameObject instancedItem;
    private GameObject parent;
    Vector3 startPosition;

    public LayerMask mask;

    public bool stairs;


    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject.Find("MainCamera").GetComponent<camera>().enabled = false;

        Debug.Log("DRAG AKTIV");
        parent = GameObject.Find("UI");
        startPosition = gameObject.transform.position;
        instancedItem = Instantiate(gameObject, parent.transform);
        instancedItem.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        instancedItem.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Destroy(instancedItem);

        GameObject.Find("MainCamera").GetComponent<camera>().enabled = true;


        Vector3 point;

        if (getTargetLocation(out point))
        {
            if (!stairs)
            {
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
                    Debug.Log("Rotation: " + startRot);
                    Instantiate(prefabPlacementObject, placentPosition, startRot);
                }
            }
            else
            {
                if (hitObject.GetComponent<Cube>().code == 5)
                {
                    Instantiate(prefabPlacementObject, hitObject.transform.position, hitObject.transform.rotation);
                }
            }
        }
    }

    bool getTargetLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
        {
                point = hitInfo.point;
                Debug.Log("Punkt: " + point);
                hitObject = hitInfo.collider.gameObject;
                hitCollider = hitInfo.collider;
                Debug.Log("GameObject: " + hitObject);
                Debug.Log("Tag: " + hitObject.tag);
                normal = hitInfo.normal;
                Debug.Log("Normal: " + normal);
                return true;
        }
        //wenn der Mauszeiger nicht auf der Fläche liegt wird false zurückgegeben 
        point = Vector3.zero;
        return false;
    }
}
