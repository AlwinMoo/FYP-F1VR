using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    //private AudioSource source;
    public string clipName;
    public AudioClip clip;

    //[Range(0f, 1f)]
    //public float volume;
    //[Range(0f, 3f)]
    //public float pitch;

    //public bool loop = false;
    //public bool playOnAwake = false;


    //public void SetSource(AudioSource _source)
    //{
    //    source = _source;
    //    source.clip = clip;
    //    source.pitch = pitch;
    //    source.volume = volume;
    //    source.playOnAwake = playOnAwake;
    //    source.loop = loop;
    //    source.outputAudioMixerGroup = audioMixerGroup;
        
    //    //Audio Source settings
    //    //source.rolloffMode = AudioRolloffMode.Logarithmic;
    //    source.spatialBlend = 1;
    //    source.maxDistance = 50;
        
    //}

    //public AudioSource GetSource()
    //{
    //    return source;
    //}


    //public void Play()
    //{
    //    source.Play();
    //}
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float volume;//general volume, may be seperated into 3 later - BGM,SFX and master

    [SerializeField]
    Sound[] sound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < sound.Length; i++)
        {
            GameObject go = new GameObject("Sound_ " + i + " " + sound[i].clipName);
            //go.transform.SetParent(this.transform);
            //sound[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    public void SetVolume(float volumeAmt)
    {
        volume = volumeAmt;
    }

    public float GetVolume()
    {
        return volume;
    }

    //can use this for background(not based off audiosource's distance) later

    //public void PlaySound(string name)
    //{
    //    for (int i = 0; i < sound.Length; i++)
    //    {
    //        if (sound[i].clipName == name)
    //        {
    //            if (!sound[i].GetSource().isPlaying)
    //            {
    //                sound[i].Play();
    //            }
    //            return;
    //        }
    //    }
    //}

    public int GetSoundLength()
    {
        return sound.Length;
    }

    public Sound GetSound(int i)
    {
        return sound[i];
    }
}
