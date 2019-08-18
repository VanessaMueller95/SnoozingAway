using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewCell
{
    // state 
    public bool enabled = false;
    // model
    public int code = 0; //0: Boden; 1: Wasser; 2: Eule; 3: Raben; 4:Treppe

    bool[] walkable = { false, false, false, false, false, false };
}
