﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Audio : MonoBehaviour {

    static Audio instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
