using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject m_goLeftController;
    public GameObject m_goRightController;

    public GameObject m_goLeftHand;
    public GameObject m_goRightHand;

    float fTimer =0;
    public float fTimeLimit = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            if (fTimer <= fTimeLimit)
            {
               m_goLeftHand.SetActive(false);
               m_goRightHand.SetActive(false);
            fTimer += Time.deltaTime;
            }
            if (fTimer >= fTimeLimit)
            {
            m_goLeftHand.SetActive(true);
            m_goRightHand.SetActive(true);

            m_goLeftController.SetActive(false);
            m_goRightController.SetActive(false);
            }
        }
}
