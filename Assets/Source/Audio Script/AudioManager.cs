using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private float master;
    private float maxVolume;

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
        maxVolume = 10;
    }

    public void Update()
    {
        float value = GameObject.FindGameObjectWithTag("SoundDial").transform.eulerAngles.z;
        value = (value < 1) ? 0 : value;//If value < 1, set value and master to 0
        value /= 360;
        master = maxVolume * value;
        Debug.Log(master);
    }

    /// <summary>
    /// set master volume
    /// </summary>
    /// <param name="volumeAmt">master volume</param>
    public void SetMasterVolume(float volumeAmt)
    {
        master = volumeAmt;
    }

    /// <summary>
    /// get master volume
    /// </summary>
    /// <returns>master volume</returns>
    public float GetMasterVolume()
    {
        return master;
    }
}
