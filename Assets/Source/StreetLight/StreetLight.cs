using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour
{
    DayNight time;
    float fCurrentHour;
    bool bCurrentAm;
    public GameObject m_StreetLight;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Sun = GameObject.FindGameObjectWithTag("Sun");
        time = Sun.GetComponent<DayNight>();
    }

    // Update is called once per frame
    void Update()
    {
        fCurrentHour = time.ReturnHour();
        bCurrentAm = time.ReturnAM();
        if (fCurrentHour == 7 && bCurrentAm == true)
        {
            m_StreetLight.SetActive(false);
        }
        if (fCurrentHour == 7 && bCurrentAm == false)
        {
            m_StreetLight.SetActive(true);
        }
    }
}
