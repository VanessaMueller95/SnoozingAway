using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;


/*
 Das ist hier erstmal so wie man es nicht machen soll: alles in einem Skript 

eine Voxelstruktur in der die Charaktere sich bewegen. 

 */

public class Cuboid : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        // state 
        public bool enabled = false;
        // model
        public int code = 0; //0: Boden; 1: Wasser; 2: Eule; 3: Raben; 4:Treppe

        bool[] walkable = {false, false, false, false, false, false};
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

    private Cursor EditorCursor = new Cursor();

    // store cell data - temporary storage also for editor
    private string cellDataFile = "level1.dat";

    [HideInInspector]
    public bool levelLoaded = false;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        coroutine = DownloadFile();
        yield return StartCoroutine(coroutine);

        Debug.Log(Application.persistentDataPath);

        // create cells - welcome to Unity vectors having no trace
        cells = new Cell[dimensions.x * dimensions.y * dimensions.z];
        Debug.Log(cells.Length);

        for(int i = 0; i < cells.Length;i++) {
            cells[i] = new Cell();
        }

        Read();

        UpdateVisuals();

    }

    // Update is called once per frame
    void Update()
    {

        // this should go into some update cursor method ...
        var updatedCursorPos = EditorCursor.pos;

        // general stuff
        if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("Löschen");
            Remove();
            UpdateVisuals();
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            changePlacementObject();
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            Application.Quit();
        } else 
        // generate random world
        if (Input.GetKeyUp(KeyCode.G))
        {
            Randomize();
            //UpdateVisuals();
        } else if (Input.GetKeyUp(KeyCode.C)) {
            // toggle cursor
            EditorCursor.enabled = !EditorCursor.enabled;
        }
        // save stuff
        else if (Input.GetKeyUp(KeyCode.S))
        {
            Save();
        }
        // read and visualize
        else if (Input.GetKeyUp(KeyCode.R))
        {
            Read();
            UpdateVisuals();
        }
        /* minimal editor */
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
            cells[EditorCursor.pos].code = EditorCursor.code;
            cells[EditorCursor.pos].enabled = !cells[EditorCursor.pos].enabled;
            UpdateVisuals();
        }

        // just in case
        cursorShape.GetComponent<Renderer>().enabled = EditorCursor.enabled;

        // clamp
        EditorCursor.pos = Mathf.Clamp(updatedCursorPos,0,cells.Length-1);

        // visualize cursor if applicable
        var pos_i = GetPosition(EditorCursor.pos, dimensions);
        Vector3 p = (Vector3)pos_i * cellSize - CenterPoint;

        cursorShape.transform.localPosition = p;
    }

    void Remove()
    {
        Debug.Log("in Methode");
        foreach (Cell c in cells)
        {
            c.enabled = false;
        }
    }

    public void Randomize()
    {
        // just for debugging
        foreach(Cell c in cells)
        {
            c.code = Random.Range(0, cellObjects.Length);
            c.enabled = true;
        }
        UpdateVisuals();
    }

    //Methode, um das Objekt zu wechseln, das plaziert wird
    void changePlacementObject()
    {
        if(EditorCursor.code + 1 < cellObjects.Length)
        {
            EditorCursor.code++;
        }
        else
        {
            EditorCursor.code = 0;
        }
        Debug.Log("Aktuelles Objekt: " + EditorCursor.code);
        Debug.Log("0: Boden; 1: Wasser; 2: Eule; 3: Raben; 4:Treppe");
    }

    void UpdateVisuals()
    {
        // delete all children
        gameObject.DeleteAllChildren();

        // build visual representation
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].enabled) 
            {

                var pos_i = GetPosition(i, dimensions);

                Vector3 p = (Vector3)pos_i * cellSize - CenterPoint;

                var item = cellObjects[cells[i].code];

                if (item)
                {
                    var cellObject = Instantiate(item, p, Quaternion.identity);

                    cellObject.transform.parent = transform;
                    // cellObject.transform.localPosition = p;

                    cellObject.name = "cell_" + pos_i.x + "_" + pos_i.y + "_" + pos_i.z;

                    var cellSkript = cellObject.GetComponent<Cube>();

                    cellSkript.code = cells[i].code;
                    cellSkript.enabled = true;
                    cellSkript.walkable[0] = false;
                    cellSkript.walkable[1] = false;
                    cellSkript.walkable[2] = false;
                    cellSkript.walkable[3] = false;
                    cellSkript.walkable[4] = false;
                    cellSkript.walkable[5] = false;
                }
            }
        }
    }

    void Save()
    {
        string dataPath = Path.Combine(Application.streamingAssetsPath, cellDataFile);

        FileStream file;

        file = File.Exists(dataPath) ? File.OpenWrite(dataPath) : File.Create(dataPath);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, cells);

        file.Close();
    }
    static IEnumerator DownloadFile()
    {
        Debug.Log("TEEEEEEEEEEEEEEEEEST");
        UnityWebRequest uwr;
        uwr = new UnityWebRequest(Path.Combine(Application.streamingAssetsPath, "level1.dat"));
        uwr.downloadHandler = new DownloadHandlerFile(Path.Combine(Application.persistentDataPath, "level1.dat"));
        yield return uwr.SendWebRequest();
        Debug.Log(Path.Combine(Application.streamingAssetsPath, "level1.dat") + " File successfully downloaded and saved to " + Path.Combine(Application.persistentDataPath, "level1.dat"));
        
    }


    bool Read()
    {

        string dataPath = Path.Combine(Application.persistentDataPath, "level1.dat");
        Debug.Log(dataPath);

        FileStream file;

        if (File.Exists(dataPath))
        {
            file = File.OpenRead(dataPath);
        }
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();

   

        cells = (Cell[])bf.Deserialize(file);

        file.Close();

        levelLoaded = true;

        return true;
    }

    public int CellCount 
    {
        get { return dimensions.x * dimensions.y * dimensions.z; }
    }

    public int GetEnabledCount {
        get { 
            int count = 0;
            foreach(Cell c in cells) if (c.enabled) count++;
            return count; 
        }
    }

    public Cell[] Cells
    {
        get { return cells; } 
    }

    public Vector3Int Dimensions
    {
        get { return dimensions; }
    }

    public Vector3 CenterPoint 
    {

        get { return new Vector3(dimensions.x * cellSize * 0.5f,
                dimensions.y * cellSize * 0.5f,
                dimensions.z * cellSize * 0.5f); }
    }

    static int GetOffset(Vector3Int pos, Vector3Int dim)
    {
        return pos.x + pos.y * dim.x + pos.z * dim.x * dim.y;
    }

    public static Vector3Int GetPosition(int idx, Vector3Int dim)
    {
        return new Vector3Int(idx % dim.x, (idx / dim.x) % dim.y, idx / (dim.x * dim.y));
    }
}
