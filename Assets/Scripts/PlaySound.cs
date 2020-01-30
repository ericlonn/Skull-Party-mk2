using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public List<Sound> audioClips = new List<Sound>();

    void Awake()
    {
        foreach (Sound sound in audioClips)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.playOnAwake = false;
        }
    }

    public void PlayClip(int clipID, bool preventOvelap)
    {
        if (audioClips[clipID].audioSource.isPlaying && preventOvelap)
        {
            return;
        }
        else
        {
            audioClips[clipID].audioSource.volume = audioClips[clipID].volume;
            audioClips[clipID].audioSource.Play();
        }
    }

    public void StopClip(int clipID)
    {
        audioClips[clipID].audioSource.Stop();
    }
}
