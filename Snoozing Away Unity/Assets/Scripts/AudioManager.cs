using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    //Nutzung der Soundklasse in einem Array für alle Audioclips
    public Sound[] sounds;
    public static AudioManager instance;

    //Slider für das Mastervolume
    [Range(0f, 1f)]
    public float MasterVolume;

    //Slider für das Effectsvolume
    [Range(0f, 1f)]
    public float EffectsVolume;

    // Use this for initialization
    void Awake () {

        //Da der Audiomanager in Szenen übernommen wird muss getestet werden ob bereits einer vorhanden ist
        //Falls es bereits ein Audimanager gibt wird der neue zerstört
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Verhindert das Zerstöhren des Objekts bei dem Übergang in eine neue Szene
        DontDestroyOnLoad(gameObject);

        //Übernimmt die im Array eingestellten Daten
		foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * MasterVolume;
            s.source.loop = s.loop;
        }
	}

    //startet direkt die Hintergrundmusik
    void Start()
    {
        Play("BackgroundMusic"); 
    }

    //Übernimmt Veränderungen der Lautstärke
    public void Update()
    {
        foreach (Sound s in sounds)
        {
            if (s.effect)
            {
                s.source.volume = s.volume * EffectsVolume;
            } 
            else
            {
                s.source.volume = s.volume * MasterVolume;
            }
        }
    }

    //Play Methode, die von überall Audioclips abspielen kann
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    //Stop Methode, die von überall Audioclips abspielen kann
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
}
