using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerInit : MonoBehaviour
{
    private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        Player.transform.SetParent(GameObject.FindWithTag("Car").transform);
        Player.transform.localPosition = new Vector3(-0.351f, -0.24f, -0.02f);
        Player.transform.localEulerAngles = new Vector3(0, -10, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
