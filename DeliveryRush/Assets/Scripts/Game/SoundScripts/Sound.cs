using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;

    [Range(0f,1f)]
    public float volume;
    [Range(0f,2f)]
    public float pitch;

    public AudioClip soundClip;
    public bool isLooping;

    [HideInInspector] public AudioSource source;
}
