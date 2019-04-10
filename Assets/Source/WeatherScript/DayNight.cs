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

    bool bAm = true;

    void Start()
    {
        fAngle = 4f * Time.fixedDeltaTime;
    }

    void Update()
    {
        Debug.Log(fHourAngle + " " + fRealHour + " am:" + bAm);
        transform.RotateAround(Vector3.zero, Vector3.right, fAngle);
        frealAngle += fAngle;
        fHourAngle += fAngle;
        if (frealAngle >= 360)
        {
            frealAngle = 0f;
            nDay += 1;
        }
        if (fHourAngle >= 15)
        {
            fRealHour += 1;
            fHourAngle = 0;
        }
        if (fRealHour >= 13)
        {
            fRealHour -= 12;
            if(bAm == true)
            {
                bAm = false;
            }
            else
                bAm = true; 
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

    public float ReturnHour()
    {
        // return fDay;
        return fRealHour;
    }

    public bool ReturnAM()
    {
        return bAm;
    }
}

