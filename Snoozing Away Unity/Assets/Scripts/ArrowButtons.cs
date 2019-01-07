using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButtons : MonoBehaviour
{
    //Variablen für den Boden und die verschiedenen Pfeil-Prefabs 
    //Werden über Editor eingestellt 
    public GameObject floor;
    public GameObject prefabLeft;
    public GameObject prefabRight;
    public GameObject prefabForward;
    public GameObject prefabBack;

    //OnClick Funktionen für verschiedene Pfeilbuttons im UI
    //Aktiviert das FloorObjectPlacement Script und setzt die passenden Prefabs
    public void OnClickForward()
    {
        Debug.Log("Vorwärts-Pfeil gedrückt");
        floor.GetComponent<FloorObjectPlacement>().enabled = true;
        floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
    }

    public void OnClickBackward()
    {
        Debug.Log("Rückwärts-Pfeil gedrückt");
        floor.GetComponent<FloorObjectPlacement>().enabled = true;
        floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
    }

    public void OnClickLeft()
    {
        Debug.Log("Links-Pfeil gedrückt");
        floor.GetComponent<FloorObjectPlacement>().enabled = true;
        floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
    }

    public void OnClickRight()
    {
        Debug.Log("Rechts-Pfeil gedrückt");
        floor.GetComponent<FloorObjectPlacement>().enabled = true;
        floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
    }
}
