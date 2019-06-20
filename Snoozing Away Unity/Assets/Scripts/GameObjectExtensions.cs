using System.Collections;
using System.Collections.Generic;
using UnityEngine;
static public class GameObjectExtensions
{    
    static public void DeleteAllChildren(this GameObject go)
    {
        // using mnemonic loop backwards - don't try to reverse!
        for (int i = go.transform.childCount; i --> 0; )
            GameObject.Destroy(go.transform.GetChild(i).gameObject);
    }  
}