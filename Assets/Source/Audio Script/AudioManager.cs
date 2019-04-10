using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private float master;
    private float SFXVolume;
    private float BGMVolume;

    private float SFXMultiplier;
    private float BGMMultiplier;

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
        master = 10;
        SFXMultiplier = 10;
        BGMMultiplier = 10;
    }

    public void Update()
    {
        SFXVolume = master * SFXMultiplier / 100;
        if (master != 0 && SFXMultiplier != 0)
        {
            if (SFXVolume < 0.1)
            {
                SFXVolume = 0.1f;
            }
        }

        BGMVolume = master * BGMMultiplier / 100;
        if (master != 0 && BGMMultiplier != 0)
        {
            if (BGMVolume < 0.1)
            {
                BGMVolume = 0.1f;
            }
        }
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
    /// set sfx volume
    /// </summary>
    /// <param name="volumeAmt">sfx volume</param>
    public void SetSFXVolume(float volumeAmt)
    {
        SFXVolume = volumeAmt;
    }

    /// <summary>
    /// set bgm volume
    /// </summary>
    /// <param name="volumeAmt">bgm volume</param>
    public void SetBGMVolume(float volumeAmt)
    {
        BGMVolume = volumeAmt;
    }

    /// <summary>
    /// set sfx multiplier
    /// </summary>
    /// <param name="amt">sfx multiplier</param>
    public void SetSFXMultiplier(float amt)
    {
        SFXMultiplier = amt;
    }

    /// <summary>
    /// set bgm multiplier
    /// </summary>
    /// <param name="amt">bgm multiplier</param>
    public void SetBGMMultiplier(float amt)
    {
        BGMMultiplier = amt;
    }

    /// <summary>
    /// get master volume
    /// </summary>
    /// <returns>master volume</returns>
    public float GetMasterVolume()
    {
        return master;
    }

    /// <summary>
    /// get sfx volume
    /// </summary>
    /// <returns>sfx volume</returns>
    public float GetSFXVolume()
    {
        return SFXVolume;
    }

    /// <summary>
    /// get bgm volume
    /// </summary>
    /// <returns>bgm volume</returns>
    public float GetBGMVolume()
    {
        return BGMVolume;
    }

    /// <summary>
    /// get sfx multiplier
    /// </summary>
    /// <returns>sfx multiplier</returns>
    public float GetSFXMultiplier()
    {
        return SFXMultiplier;
    }

    /// <summary>
    /// get bgm multiplier
    /// </summary>
    /// <returns>bgm multiplier</returns>
    public float GetBGMMultiplier()
    {
        return BGMMultiplier;
    }
}
