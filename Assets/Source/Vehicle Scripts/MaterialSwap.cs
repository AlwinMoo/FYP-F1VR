using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwap : MonoBehaviour
{
    public Material normal;
    public Material white;
    public Material red;
    public Material yellow;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "RedLight")
            {
                transform.GetChild(i).GetComponent<Renderer>().material = red;
            }
        }
    }
}
