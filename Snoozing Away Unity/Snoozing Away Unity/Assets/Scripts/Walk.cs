using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    private Animator animator;

    private Cuboid cuboid;

    private int currentPos;
    private int currentSide = 0;

    private Vector3 [] sideDirections = {
        Vector3.up,Vector3.left,Vector3.right,Vector3.down,Vector3.forward,Vector3.back
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cuboid = FindObjectOfType<Cuboid>();

        Reset();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            Reset();

        } else if (Input.GetKeyUp(KeyCode.A)) {
            currentSide = (currentSide + 1) % sideDirections.Length;
            Debug.Log("Side:" + currentSide);
        }

        // internal stuff
        UpdateObstacles();

        // update transforms
        UpdateVisuals();

    }


    void UpdateObstacles()
    {

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * cuboid.cellSize, Color.yellow);
        // Debug.DrawRay(transform.position, Quaternion.Inverse(transform.rotation) * Vector3.down * cuboid.cellSize, Color.blue);

        // get floor hit
        RaycastHit hit;

        if (Physics.Raycast(gameObject.transform.position,Vector3.down  * cuboid.cellSize,out hit,cuboid.cellSize * 2.0f)) {

            Debug.Log(hit.normal);

        }
    }

    void UpdateVisuals()
    {
        // this is only an initial method - update to use original raycasting

        // get side vector
        Vector3 sideVector = sideDirections[currentSide];

        // initial position from this point out we should cast the rays 
        Vector3 worldPos = (Vector3)Cuboid.GetPosition(currentPos,cuboid.Dimensions) * cuboid.cellSize - cuboid.CenterPoint;

        // 
        gameObject.transform.localPosition = worldPos + sideVector * cuboid.cellSize; // basically we occupy the other cell
        //gameObject.transform.localRotation = Quaternion.Inverse(Quaternion.LookRotation(sideVector));

    }

    public void Reset()
    {

        Debug.Log("enabled: " + cuboid.GetEnabledCount);

        for (var i = 0; i < cuboid.CellCount; i++) 
        {
            // the snoozer can be only attached to enabled cells 
            // needs to be revised ...
            if (cuboid.Cells[i].enabled == true) 
            {
                Debug.Log("found cell:" + i); 
                
                // need to check walkable or not 

                currentPos = i;

                return; // stop here
            }
        }
    }
}
