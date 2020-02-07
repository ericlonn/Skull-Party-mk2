using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicClip
{
    public AudioClip musicClip;
    public float loopFadeTime;
    public bool looping;
    [HideInInspector]
    public AudioSource audioSource;
}
