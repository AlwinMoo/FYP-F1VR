using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {

    float FDay = 1.2f;
    float angle;

	// Use this for initialization
	void Start () {
        angle = 5f * Time.deltaTime;

    }
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(Vector3.zero, Vector3.right, angle);
        FDay += angle;



    }

    public float ReturnDay()
    {
        return FDay;
    }
}
