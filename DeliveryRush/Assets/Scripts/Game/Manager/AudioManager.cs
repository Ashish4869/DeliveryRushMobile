using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Assigns and Plays audio
    /// </summary>
    
    [SerializeField]
    Sound[] sounds; //holds all the sounds that are played in the game

    private void Awake()
    {
        foreach  (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.clip = s.soundClip;
            s.source.loop = s.isLooping;
        }
    }

   public void Play(string name) // plays the sound referenced by the string
   {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            return;
        }

        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
   }

    public void Stop(string name) //Stops the sound that is currently playing 
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    private void Start()
    {
        Play("BackGroundNoise");
        Play("BGMUSIC");
    }
}
