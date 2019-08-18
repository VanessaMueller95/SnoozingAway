using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
public class LevelLoading : MonoBehaviour
{
    private string cellDataFile = "/level1.dat";

    [HideInInspector]
    public bool levelLoaded = false;
    
    bool Read()
    {

        string dataPath = "Assets/LevelData" + cellDataFile;
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

    void Save()
    {
        string dataPath = "Assets/LevelData" + cellDataFile;

        FileStream file;

        file = File.Exists(dataPath) ? File.OpenWrite(dataPath) : File.Create(dataPath);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, cells);

        file.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/