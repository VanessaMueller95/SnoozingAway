using UnityEngine.Audio;
using UnityEngine;

//Sound Klasse, die im Audiomanager verwendet wird

//Soll im Inspektor sichtbar sein
[System.Serializable]
public class Sound{

    //Möglichkeit verschiedene Variablen festzulegen, die im Audioclip gebraucht werden
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;

    public bool loop;
    public bool effect;

    [HideInInspector]
    public AudioSource source;
}
