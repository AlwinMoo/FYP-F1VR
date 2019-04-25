using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    GameObject go_Sun;
    GameObject go_Moon;
    GameObject go_SunSetting;
    GameObject go_MoonSetting;
    Transform tr_Sun;
    float frealAngle = 4f;
    float fAngle;
    float fHourAngle;
    float fRealHour = 7;
    int nDay = 1;

    Vector3 SunPos;
    Vector3 MoonPos;
    bool bAm = true;

    bool bOnceD = true;
    bool bOnceN = true;

    void Start()
    {
        fAngle = 4f * Time.fixedDeltaTime *2;
        go_Sun = GameObject.FindGameObjectWithTag("Sun");
        SunPos = go_Sun.transform.position;
        //go_Moon = GameObject.FindGameObjectWithTag("Moon");
        //MoonPos = SunPos;

        go_SunSetting = GameObject.FindGameObjectWithTag("DaySetting");
        //go_MoonSetting = GameObject.FindGameObjectWithTag("NightSetting");
    }

    void Update()
    {
        Debug.Log(frealAngle + " Time --" + fHourAngle + " " + fRealHour + " am:" + bAm);
        frealAngle += fAngle;
        fHourAngle += fAngle;
        if (go_Sun != null)
        {
            go_Sun.transform.RotateAround(Vector3.zero, Vector3.right, fAngle);
        }

        //if (go_Moon != null)
        //{
        //    go_Moon.transform.RotateAround(Vector3.zero, Vector3.right, fAngle);
        //}

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
            if (bAm == true)
            {
                bAm = false;
            }
            else
                bAm = true;
        }
        if (fRealHour == 6 && bAm == true)
        {
            //if (bOnceD)
            //{
            //    go_Sun.transform.position = SunPos;
            //    bOnceD = false;
            //}
            go_SunSetting.SetActive(true);
           // go_MoonSetting.SetActive(false);
            go_Sun.SetActive(true);
           // go_Moon.SetActive(false);
        }
        if (fRealHour == 7 && bAm == true)
        {
            bOnceD = true;
        }
        if (fRealHour == 8 && bAm == false)
          {
            //if (bOnceN)
            //{
            //    go_Moon.transform.position = MoonPos;
            //    bOnceN = false;
            //}
            go_SunSetting.SetActive(false);
            //go_MoonSetting.SetActive(true);
            go_Sun.SetActive(false);
            //go_Moon.SetActive(true);
        }
        if (fRealHour == 9 && bAm == false)
        {
            bOnceN = true;
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

