using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogueSpeedoMeter : MonoBehaviour
{
    static float minAngle = 122.0f;
    static float maxAngle = -58f;
    static AnalogueSpeedoMeter theSpeedo;

    // Start is called before the first frame update
    void Start()
    {
        theSpeedo = this;
    }

    public static void SpeedToAngle(float speed, float min, float max)
    {
        float angle = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(min, max, speed));
        theSpeedo.transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
