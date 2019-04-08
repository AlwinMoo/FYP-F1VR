using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {

    float fDay = 1.2f;
    float fAngle;

	void Start () {
        fAngle = 5f * Time.deltaTime;
    }
	
	void Update () {
        transform.RotateAround(Vector3.zero, Vector3.right, fAngle);
        fDay += fAngle;
    }

    /// <summary>
    /// Get current day.
    /// </summary>
    /// <returns> Returns the current day</returns>
    public float ReturnDay()
    {
        return fDay;
    }
}
