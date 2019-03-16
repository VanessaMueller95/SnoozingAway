using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;

    [Range(0f, 1f)]
    public float MasterVolume;

    [Range(0f, 1f)]
    public float EffectsVolume;

    // Use this for initialization
    void Awake () {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

		foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * MasterVolume;
            s.source.loop = s.loop;
        }
	}

    void Start()
    {
        Play("BackgroundMusic"); 
    }

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

    // Update is called once per frame
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
