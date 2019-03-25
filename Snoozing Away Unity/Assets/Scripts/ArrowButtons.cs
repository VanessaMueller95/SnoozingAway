using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButtons : MonoBehaviour
{
    //Variablen für den Boden und die verschiedenen Pfeil-Prefabs 
    //Werden über den Inspektor eingestellt 
    public GameObject floor;
    public GameObject prefabLeft;
    public GameObject prefabRight;
    public GameObject prefabForward;
    public GameObject prefabBack;

    public string actualButton;


    //OnClick Funktionen für verschiedene Pfeilbuttons im UI
    //Aktiviert das FloorObjectPlacement Script und setzt die passenden Prefabs
    public void OnClickDirectionButton(string button)
    {
        //Speicherung welcher Button die Funktion aufruft
        actualButton = button;

        //Sollen Treppen plaziert werden, werden alle nötigen Skripte der Treppenplatzhalter aktiviert
        if (actualButton == "Stairs") {

            //Skript für die Platzierung der Pfeilfelder wird deaktiviert
            DeactivateArrows();

            //Skript für die Treppen aktiviert
            ActivateStairs();
        }

        //Vorwärtspfeil
        if (actualButton == "Forward")
        {
            Debug.Log("Vorwärts-Pfeil gedrückt");

            //Aktivierung/Deaktivierung des nötigen Platzierungs-Skripte
            ActivateArrows();
            DeactivateStairs();

            //Aktualisierung des Prefabs in Abhängigkeit der Kamerarichtung
            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    Debug.Log("in Case Block-front");
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    Debug.Log("in Case Block-left");
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    Debug.Log("in Case Block-right");
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    Debug.Log("in Case Block-back");
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    Debug.Log("in Case Block-default");
                    break;
            }
        }

        //Rückwärtspfeil
        else if (actualButton == "Backward")
        {
            Debug.Log("Rückwärts-Pfeil gedrückt");

            ActivateArrows();
            DeactivateStairs();

            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
            }
        }

        //Links-Pfeil
        else if (actualButton == "Left")
        {
            Debug.Log("Linker-Pfeil gedrückt");

            ActivateArrows();
            DeactivateStairs();

            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
            }
        }

        //Rechts-Pfeil
        else if (actualButton == "Right")
        {
            Debug.Log("Rechter-Pfeil gedrückt");

            ActivateArrows();
            DeactivateStairs();

            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
            }
        }
    }

    //Funktion um die Treppen zu deaktivieren
    public void DeactivateStairs()
    {
        GameObject.Find("Plazierung_Treppen1").GetComponent<FloorObjectPlacement>().enabled = false;
        GameObject.Find("Plazierung_Treppen2").GetComponent<FloorObjectPlacement>().enabled = false;
        GameObject.Find("Plazierung_Treppen3").GetComponent<FloorObjectPlacement>().enabled = false;
        GameObject.Find("Plazierung_Treppen4").GetComponent<FloorObjectPlacement>().enabled = false;
        GameObject.Find("Plazierung_Brücken1").GetComponent<FloorObjectPlacement>().enabled = false;
        GameObject.Find("Plazierung_Brücken1_1").GetComponent<FloorObjectPlacement>().enabled = false;
        GameObject.Find("Plazierung_Brücken1_2").GetComponent<FloorObjectPlacement>().enabled = false;
    }

    //Funktion um die Treppen zu aktivieren
    public void ActivateStairs()
    {
        GameObject.Find("Plazierung_Treppen1").GetComponent<FloorObjectPlacement>().enabled = true;
        GameObject.Find("Plazierung_Treppen2").GetComponent<FloorObjectPlacement>().enabled = true;
        GameObject.Find("Plazierung_Treppen3").GetComponent<FloorObjectPlacement>().enabled = true;
        GameObject.Find("Plazierung_Treppen4").GetComponent<FloorObjectPlacement>().enabled = true;
        GameObject.Find("Plazierung_Brücken1").GetComponent<FloorObjectPlacement>().enabled = true;
        GameObject.Find("Plazierung_Brücken1_1").GetComponent<FloorObjectPlacement>().enabled = true;
        GameObject.Find("Plazierung_Brücken1_2").GetComponent<FloorObjectPlacement>().enabled = true;
    }

    //Funktion um die Pfeile zu deaktivieren
    public void DeactivateArrows()
    {
        floor.GetComponent<FloorObjectPlacement>().enabled = false;
    }

    //Funktion um die Pfeile zu aktivieren
    public void ActivateArrows()
    {
        floor.GetComponent<FloorObjectPlacement>().enabled = true;
    }
}
