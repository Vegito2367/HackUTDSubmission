using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Sound 
{
    public string SoundName;
    public AudioClip Clip;
    public float pitch;
    [HideInInspector]
    public AudioSource audioSource;
    [Range(0f,1f)]
    public float Volume;
    public bool loop;
    public AudioMixerGroup Output;
    [Range(0f,1f)]
    public float SpacialBlend;
}
