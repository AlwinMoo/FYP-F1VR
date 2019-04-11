using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrafficLight : MonoBehaviour
{

    public GameObject TrafficLightObject;
    public GameObject TrafficLightObject2;

    public enum LIGHT_STATUS
    {
        LIGHT_RED,
        LIGHT_GREEN,
        LIGHT_YELLOW,

        NUM_OF_LIGHTS
    };
    public LIGHT_STATUS trafficLight;

    float trafficCD;
    float yellowCD;

    public bool IsSplitPoint = false;

    //DEBUG
    GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));

        trafficLight = (LIGHT_STATUS)(Random.Range(0, 2));

        trafficCD = 0f;
        yellowCD = 0f;

        //spawn debug cube only in debug mode
        if (Debug.isDebugBuild)
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //-------DEBUG CUBE LIGHTS-------
        if (Debug.isDebugBuild)
        {
            cube.transform.position = this.transform.position;
            switch (trafficLight)
            {
                case LIGHT_STATUS.LIGHT_GREEN:
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    break;
                case LIGHT_STATUS.LIGHT_RED:
                    cube.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case LIGHT_STATUS.LIGHT_YELLOW:
                    cube.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                default:
                    break;
            }
        }
        //-------DEBUG CUBE LIGHTS END-------

        //-------TRAFFIC LIGHT CONTROLLER-------
        if (TrafficLightObject != null && TrafficLightObject2 != null)
        {
            LightReset();
            switch (trafficLight)
            {
                case LIGHT_STATUS.LIGHT_RED:
                    {
                        TrafficLightObject.transform.GetChild(1).gameObject.SetActive(true);
                        TrafficLightObject2.transform.GetChild(1).gameObject.SetActive(true);
                        break;
                    }

                case LIGHT_STATUS.LIGHT_YELLOW:
                    {
                        TrafficLightObject.transform.GetChild(2).gameObject.SetActive(true);
                        TrafficLightObject2.transform.GetChild(2).gameObject.SetActive(true);
                        break;
                    }
                case LIGHT_STATUS.LIGHT_GREEN:
                    {
                        TrafficLightObject.transform.GetChild(3).gameObject.SetActive(true);
                        TrafficLightObject2.transform.GetChild(3).gameObject.SetActive(true);
                        break;
                    }
                default:
                    break;
            }
        }
        //-------TRAFFIC LIGHT CONTROLLER END-------


        //-------LIGHT LOGIC-------
        trafficCD += Time.deltaTime;

        if (trafficCD >= 10f)
        {
            if (trafficLight == LIGHT_STATUS.LIGHT_GREEN)
            {
                trafficLight = LIGHT_STATUS.LIGHT_YELLOW;
            }
            else if (trafficLight == LIGHT_STATUS.LIGHT_RED)
            {
                trafficLight = LIGHT_STATUS.LIGHT_GREEN;
                trafficCD = 0f;
            }
            else if (trafficLight == LIGHT_STATUS.LIGHT_YELLOW)
            {
                yellowCD += Time.deltaTime;

                if (yellowCD >= 3f)
                {
                    trafficLight = LIGHT_STATUS.LIGHT_RED;
                    trafficCD = 0f;
                    yellowCD = 0f;
                }
            }

            return;
        }
        //-------LIGHT LOGIC END-------



    }

    void LightReset()
    {
        TrafficLightObject.transform.GetChild(1).gameObject.SetActive(false);
        TrafficLightObject.transform.GetChild(2).gameObject.SetActive(false);
        TrafficLightObject.transform.GetChild(3).gameObject.SetActive(false);
        TrafficLightObject2.transform.GetChild(1).gameObject.SetActive(false);
        TrafficLightObject2.transform.GetChild(2).gameObject.SetActive(false);
        TrafficLightObject2.transform.GetChild(3).gameObject.SetActive(false);
    }



}
