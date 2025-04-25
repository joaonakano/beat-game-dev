using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Standards;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
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

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }


    void Start()
    {
        Play("");
    }

    public void Play (string name)
    {
       Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if(s == null)
        {
            Debug.LogWarning("Efeito: " + name + "Não achei");
            return;
        }
        s.audioSource.Play();
    }
}
