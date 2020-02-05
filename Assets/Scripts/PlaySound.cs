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

    public void PlayClip(int clipID, bool preventOvelap, Vector3 location = default(Vector3))
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

        if (location != Vector3.zero)
        {
            float cameraBounds = Camera.current.scaledPixelWidth / 2;
            float clipXPos = Camera.current.WorldToScreenPoint(location).x;

            if (clipXPos < cameraBounds)
            {
                audioClips[clipID].audioSource.panStereo = Mathf.Lerp(-1, 0, clipXPos / cameraBounds);
            }
            else
            {
                audioClips[clipID].audioSource.panStereo = Mathf.Lerp(0, 1, (clipXPos - cameraBounds) / cameraBounds);
            }
        }
    }

    public void StopClip(int clipID)
    {
        audioClips[clipID].audioSource.Stop();
    }

}
