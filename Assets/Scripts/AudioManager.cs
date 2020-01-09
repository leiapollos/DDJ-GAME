﻿using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    protected bool gameEnded = false;

    void Awake()
    {
        //ADD THIS IF YOU WANT THE THEME TO CONTINUE THROUGH SCENES:

        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        ////////////////////////////////////////////////////////////////////////
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
        }
    }

    private void Start()
    {
        Play("MainTheme");
        gameEnded = false;
    }
    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found.");
            return;
        }
        s.source.Play();
    }

    public void PlayVaried(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found.");
            return;
        }

        s.source.pitch = 1 + (float)UnityEngine.Random.Range(-0.2f, 0.2f);
        s.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return false;
        if (s.source.isPlaying)
        {
            return true;
        }
        else return false;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found.");
            return;
        }
        if (s.source.isPlaying)s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
        gameEnded = true;
    }
}
