using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    GameObject go_Sun;
    Transform tr_Sun;
    float frealAngle = 4f;
    float fAngle;
    float fHourAngle;
    float fRealHour = 7;
    int nDay = 1;

    bool bAm = true;

    void Start()
    {
        fAngle = 4f * Time.fixedDeltaTime;
        go_Sun = GameObject.FindGameObjectWithTag("Sun");
    }

    void Update()
    {
        Debug.Log(frealAngle+ " Time --" +fHourAngle + " " + fRealHour + " am:" + bAm);
        go_Sun.transform.RotateAround(Vector3.zero, Vector3.right, fAngle);
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

        if (fRealHour == 8 && bAm == true)
        {
            go_Sun.SetActive(true);
        }
        if (fRealHour == 6 && bAm == false)
        {
            go_Sun.SetActive(false);
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

