using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnOffCar : MonoBehaviour
{
    [NonSerialized]
    public bool bOnOff = false;

    bool bOnce = false;

    public void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.name);

        bOnce = !bOnce;
    }

    public void FixedUpdate()
    {
        if (bOnce)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.green;
    }
}