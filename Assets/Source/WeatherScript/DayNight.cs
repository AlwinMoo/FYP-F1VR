using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {

    float FDay = 1.2f;
    float angle;

	void Start () {
        angle = 5f * Time.deltaTime;
    }
	
	void Update () {
        transform.RotateAround(Vector3.zero, Vector3.right, angle);
        FDay += angle;
    }

    /// <summary>
    /// Get current day.
    /// </summary>
    /// <returns> Returns the current day</returns>
    public float ReturnDay()
    {
        return FDay;
    }
}
