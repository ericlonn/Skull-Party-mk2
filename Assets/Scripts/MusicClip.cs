using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicClip
{
    public AudioClip musicClip;
    public float loopFadeTime;
    [HideInInspector]
    public AudioSource audioSource1;
    [HideInInspector]
    public AudioSource audioSource2;
}
