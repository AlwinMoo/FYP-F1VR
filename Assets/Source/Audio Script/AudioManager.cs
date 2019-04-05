using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float volume;

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
        }
    }

    /// <summary>
    /// volume set
    /// </summary>
    /// <param name="volumeAmt">volume</param>
    public void SetVolume(float volumeAmt)
    {
        volume = volumeAmt;
    }

    /// <summary>
    /// volume get
    /// </summary>
    /// <returns>volume</returns>
    public float GetVolume()
    {
        return volume;
    }

    /// <summary>
    /// gets no of sounds in list
    /// </summary>
    /// <returns>no of sounds in list</returns>
    public int GetSoundLength()
    {
        return sound.Length;
    }

    /// <summary>
    /// gets sound in list based off the sound no
    /// </summary>
    /// <param name="i">sound no</param>
    /// <returns>gets sound in list</returns>
    public Sound GetSound(int i)
    {
        return sound[i];
    }
}
