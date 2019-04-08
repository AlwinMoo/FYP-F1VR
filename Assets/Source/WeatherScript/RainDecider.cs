using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDecider : MonoBehaviour {

    public GameObject m_goRain;
    public GameObject m_goRainDrop;
    public Light m_lSun;
    public Light m_lMoon;

    private Transform trPlayer;
    public float fWeatherHeight = 90f;

    bool RainOrNot;
    float myTimer;
    int rainchance;
    float Time2;
    bool once = true;

	// Use this for initialization
	void Start () {
        RainOrNot = false;
        myTimer = 20;
        Time2 = 0;
        GameObject goPlayerGameObject = GameObject.FindGameObjectWithTag("Car");
        trPlayer = goPlayerGameObject.transform;
    }

	// Update is called once per frame
	void Update () {
        if (Time2 >= 10)
        {
            rainchance = Random.Range(1, 5);
            if (rainchance == 3)
            {
                RainOrNot = true;
            }
            Time2 = 0;
        }

        Time2 += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.O))
        {
            RainOrNot = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            RainOrNot = false;
        }

        if (RainOrNot == true)
        {
            if (m_lSun.intensity > 0.006f)
                m_lSun.intensity -= 0.003f;
            if (m_lMoon.intensity > 0.006f)
                m_lMoon.intensity -= 0.003f;

            if (once == true)
            {
                once = false;
            }
            m_goRain.SetActive(true);
            m_goRainDrop.SetActive(true);
            if (myTimer > 0)
            {
                myTimer -= Time.deltaTime;
            }
            if (myTimer <= 0)
            {
                myTimer = 20;
                RainOrNot = false;
            }
        }
        else
        {
            m_goRain.SetActive(false);
            m_goRainDrop.SetActive(false);
            once = true;
            if (m_lSun.intensity < 0.49f)
                m_lSun.intensity += 0.005f;
            if (m_lSun.intensity > 0.49f)
                m_lSun.intensity = 0.5f;
            if (m_lMoon.intensity < 0.49f)
                m_lMoon.intensity += 0.005f;
            if (m_lMoon.intensity > 0.49f)
                m_lMoon.intensity = 0.5f;
        }

        if(m_goRain.active == true)
            m_goRain.transform.position = new Vector3(trPlayer.position.x + 62, trPlayer.position.y + fWeatherHeight, trPlayer.position.z + 47);
    }
}
