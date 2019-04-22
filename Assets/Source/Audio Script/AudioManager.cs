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
        soundDial.transform.eulerAngles = new Vector3(0, 359, 0);
    }

    public void Update()
    {
        float value = soundDial.transform.eulerAngles.y;
        value = (value < 2) ? 0 : value;//If value < 2, set value and master to 0
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
