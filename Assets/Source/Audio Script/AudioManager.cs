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
    private GameObject soundDial;

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
        soundDial = GameObject.FindGameObjectWithTag("SoundDial");
    }

    public void Update()
    {
        Vector3 soundDialRotation = soundDial.transform.localRotation.eulerAngles;
        float value = soundDialRotation.y;
        
        if (value > 90 && value < 91)
        {
            value = 0;
        }
        else if (value <= 90)
        {
            value += 270;
        }
        else if (value <= 360 && value > 90)
        {
            value -= 90;
        }

        value /= 360;
        master = maxVolume * value;
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
