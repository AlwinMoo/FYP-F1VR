using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerInit : MonoBehaviour
{
    private Transform trPlayer;
    // Start is called before the first frame update
    void Start()
    {
        trPlayer = GameObject.FindWithTag("Player").transform;
        trPlayer.transform.SetParent(GameObject.FindWithTag("Car").transform);
        trPlayer.transform.localPosition = new Vector3(-0.351f, -0.24f, -0.02f);
        trPlayer.transform.localEulerAngles = new Vector3(0, -10, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
