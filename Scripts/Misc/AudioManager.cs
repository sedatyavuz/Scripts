using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public bool playOnAwake;
    public bool loop;
    [Range(0, 1)] public float spatialBlend;
    [Range(0, 1)] public float volume = 1;
    [Range(0, 3)] public float pitch = 0.5f;

    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    
    [SerializeField] private Sound[] sounds;

    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = sound.spatialBlend;
            if (sound.playOnAwake)
                sound.source.Play();
            sound.source.loop = sound.loop;
        }
    }
    public float GetClipLength(string clipName)
    {
        Sound s = Array.Find(sounds, sound => { return sound.clip.name == clipName; });
        if (s == null)
        {
            Debug.Log("Couldn't find clip names: " + clipName);
            return -1;
        }
        return s.clip.length;
    }
    public void Play(string clipName)
    {
        Sound s = Array.Find(sounds, sound => { return sound.clip.name == clipName; });
        if (s == null)
        {
            Debug.Log("Couldn't find clip names: " + clipName);
            return;
        }
        s.source.Play();
    }
    public void PlayOneShot(string clipName)
    {
        Sound s = Array.Find(sounds, sound => { return sound.clip.name == clipName; });
        if (s == null)
        {
            Debug.Log("Couldn't find clip names: " + clipName);
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

}
