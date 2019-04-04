using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject LeftController;
    public GameObject RightController;

    public GameObject LeftHand;
    public GameObject RightHand;

    float timer =0;
    public float TimeLimit = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            if (timer <= TimeLimit)
            {
            LeftHand.SetActive(false);
            RightHand.SetActive(false);
            timer += Time.deltaTime;
            }
            if (timer >= TimeLimit)
            {
                LeftHand.SetActive(true);
                RightHand.SetActive(true);

                LeftController.SetActive(false);
                RightController.SetActive(false);
            }
        }
}
