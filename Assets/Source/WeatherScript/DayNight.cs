using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{

    float frealAngle = 4f;
    float fAngle;
    float fHourAngle;
    float fRealHour = 7;
    int nDay = 1;

    bool b_once = true;
    bool b_am = true;

    void Start()
    {
        fAngle = 2f * Time.deltaTime;
    }

    void Update()
    {
       // Debug.Log(fHourAngle + " " + fRealHour + " am:" + b_am);
        transform.RotateAround(Vector3.zero, Vector3.right, fAngle);
        frealAngle += fAngle;
        fHourAngle += fAngle;
        if (frealAngle >= 360)
        {
            frealAngle = 0f;
            nDay += 1;
            b_once = true;
        }
        if (fHourAngle >= 15)
        {
            fRealHour += 1;
            fHourAngle = 0;
        }
        if (fRealHour >= 13)
        {
            fRealHour -= 12;
            if(b_am == true)
            {
                b_am = false;
            }
            else
            b_am = true; 
        }
        // fFakeTime = fFakeTime;
        //ftime = fFakeTime / 15;
    }

    /// <summary>
    /// Get current day.
    /// </summary>
    /// <returns> Returns the current day</returns>
    public float ReturnDay()
    {
        // return fDay;
        return nDay;
    }
}

