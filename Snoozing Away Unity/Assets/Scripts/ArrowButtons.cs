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

    public string actualButton;


    //OnClick Funktionen für verschiedene Pfeilbuttons im UI
    //Aktiviert das FloorObjectPlacement Script und setzt die passenden Prefabs
    public void OnClickDirectionButton(string button)
    {
        actualButton = button;

        if (actualButton == "Forward")
        {
            Debug.Log("Vorwärts-Pfeil gedrückt");
            floor.GetComponent<FloorObjectPlacement>().enabled = true;

            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
            }
        }
        else if (actualButton == "Backward")
        {
            Debug.Log("Rückwärts-Pfeil gedrückt");
            floor.GetComponent<FloorObjectPlacement>().enabled = true;

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
        else if (actualButton == "Left")
        {
            Debug.Log("Rückwärts-Pfeil gedrückt");
            floor.GetComponent<FloorObjectPlacement>().enabled = true;

            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
            }
        }
        else if (actualButton == "Right")
        {
            Debug.Log("Rückwärts-Pfeil gedrückt");
            floor.GetComponent<FloorObjectPlacement>().enabled = true;

            switch (GameObject.Find("MainCamera").GetComponent<CameraMovement>().direction)
            {
                case "Front":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
                case "Left":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabRight;
                    break;
                case "Right":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabLeft;
                    break;
                case "Back":
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabForward;
                    break;
                default:
                    floor.GetComponent<FloorObjectPlacement>().prefabPlacementObject = prefabBack;
                    break;
            }
        }
        /*
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
        }*/
    }
}
