using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnOffCar : MonoBehaviour
{
    [NonSerialized]
    public bool bOnOff = false;

    [SerializeField]
    private Material OffMaterial;
    [SerializeField]
    private Material OnMaterial;

    bool bOnce = false;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Hands"))
            bOnOff = !bOnOff;

        if (bOnOff)
            this.gameObject.GetComponent<Renderer>().material = OnMaterial;
        else
            this.gameObject.GetComponent<Renderer>().material = OffMaterial;
    }
    
}