using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public List<Sound> audioClips = new List<Sound>();
    public AudioSource currentAudioSource;

    AudioSource newAudioSource;
    int currentTrack;
    float fadeTime;
    float fadeTimer;
    bool isFading = false;

    void Awake()
    {
        foreach (Sound sound in audioClips)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (isFading)
        {
            ApplyFading();
        }
    }

    public void PlayClip(int clipID, float newFadeTime)
    {
        if (currentAudioSource == null && !isFading)
        {
            currentAudioSource = audioClips[clipID].audioSource;
            currentAudioSource.Play();
            currentAudioSource.loop = true;
        }
        else if (currentAudioSource != null && currentAudioSource.clip != audioClips[clipID].audioClip && !isFading)
        {
            fadeTime = newFadeTime;
            fadeTimer = newFadeTime;
            newAudioSource = audioClips[clipID].audioSource;
            newAudioSource.volume = 0f;
            newAudioSource.Play();
            newAudioSource.loop = true;
            isFading = true;
        }


    }

    void ApplyFading()
    {
        if (fadeTimer > 0)
        {
            currentAudioSource.volume = Mathf.SmoothStep(0, 1, fadeTimer / fadeTime);
            newAudioSource.volume = Mathf.SmoothStep(1, 0, fadeTimer / fadeTime);
            fadeTimer -= Time.deltaTime;
        }
        else
        {
            isFading = false;
            newAudioSource.volume = 1;
            currentAudioSource = newAudioSource;
        }
    }
}
