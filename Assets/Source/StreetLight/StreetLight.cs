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
        GameObject go_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        time = go_GameManager.GetComponent<DayNight>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(time.ReturnHour());
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
