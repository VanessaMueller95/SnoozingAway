using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Cuboid : MonoBehaviour
{
    [System.Serializable]
    protected class Cell
    {
        // state 
        public bool enabled = false;
        // model
        public int code = 0;
    }

    protected class Cursor {
        public int pos = 0;
        public int code = 0;
        public bool enabled = true;
    }


    protected Cell[] cells;

    public GameObject cursorShape;
    public GameObject[] cellObjects;
    // approximately the original size
    public Vector3Int dimensions = new Vector3Int(10, 10, 10);
    public float cellSize = 2.0f;

    private Cursor cursor = new Cursor();

    // store cell data - temporary storage also for editor
    private string cellDataFile = "/cells.dat";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);

        // create cells - welcome to Unity vectors having no trace
        cells = new Cell[dimensions.x * dimensions.y * dimensions.z];

        for(int i = 0; i < cells.Length;i++) {
            cells[i] = new Cell();
        }
    }

    // Update is called once per frame
    void Update()
    {

        var updatedCursorPos = cursor.pos;

        // generate random input
        if (Input.GetKeyUp(KeyCode.G))
        {
            Randomize();
            UpdateVisuals();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            Save();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            Read();
            UpdateVisuals();
            // cursor
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            updatedCursorPos += dimensions.x;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            updatedCursorPos -= dimensions.x;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            updatedCursorPos += 1;

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            updatedCursorPos -= 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            updatedCursorPos += dimensions.x * dimensions.y;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            updatedCursorPos -= dimensions.x * dimensions.y;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            cells[cursor.pos].code = cursor.code;
            cells[cursor.pos].enabled = !cells[cursor.pos].enabled;
            UpdateVisuals();
        }

        // clamp
        cursor.pos = Mathf.Clamp(updatedCursorPos,0,cells.Length-1);

        // get midpoint of structure
        Vector3 midPoint = new Vector3(dimensions.x * cellSize * 0.5f,
            dimensions.y * cellSize * 0.5f,
            dimensions.z * cellSize * 0.5f);

        // visualize cursor if applicable
        var pos_i = GetPosition(cursor.pos, dimensions);
        Vector3 p = (Vector3)pos_i * cellSize - midPoint;

        cursorShape.transform.localPosition = p;
    }

    void Randomize()
    {
        // just for debugging
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].code = Random.Range(0, cellObjects.Length);
            cells[i].enabled = true;
        }
    }

    void UpdateVisuals()
    {
        // delete all children
        gameObject.DeleteAllChildren();

        // get midpoint of structure
        Vector3 midPoint = new Vector3(dimensions.x * cellSize * 0.5f,
            dimensions.y * cellSize * 0.5f,
            dimensions.z * cellSize * 0.5f);

        // build visual representation
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].enabled) {
                var pos_i = GetPosition(i, dimensions);

                Vector3 p = (Vector3)pos_i * cellSize - midPoint;

                var item = cellObjects[cells[i].code];

                if (item)
                {
                    var cellObject = Instantiate(item, p, Quaternion.identity);

                    cellObject.transform.parent = transform;

                    cellObject.name = "cell_" + pos_i.x + "_" + pos_i.y + "_" + pos_i.z;
                }
            }
        }
    }

    void Save()
    {
        string dataPath = Application.persistentDataPath + cellDataFile;

        FileStream file;

        file = File.Exists(dataPath) ? File.OpenWrite(dataPath) : File.Create(dataPath);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, cells);

        file.Close();
    }

    void Read()
    {

        string dataPath = Application.persistentDataPath + cellDataFile;

        FileStream file;

        if (File.Exists(dataPath))
        {
            file = File.OpenRead(dataPath);
        }
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();

        cells = (Cell[])bf.Deserialize(file);

        file.Close();
    }

    static int GetOffset(Vector3Int pos, Vector3Int dim)
    {
        return pos.x + pos.y * dim.x + pos.z * dim.x * dim.y;
    }

    static Vector3Int GetPosition(int idx, Vector3Int dim)
    {
        return new Vector3Int(idx % dim.x, (idx / dim.x) % dim.y, idx / (dim.x * dim.y));
    }
}
