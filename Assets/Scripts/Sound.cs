using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    public float volume = 1;
    [HideInInspector]
    public AudioSource audioSource;
}
