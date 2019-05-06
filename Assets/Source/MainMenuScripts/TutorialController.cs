using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class TutorialController : MonoBehaviour
    {
        public GameObject m_goLeftController;
        public GameObject m_goRightController;

        public GameObject m_goLeftHand;
        public GameObject m_goRightHand;

        float fTimer = 0;
        public float fTimeLimit = 5;

        bool bControlConnected;

        // Start is called before the first frame update
        void Start()
        {
            bControlConnected = false;
        }

        void OnEnable()
        {
            Debug.Log("Testing connection for devices");
            SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);
        }

        // A SteamVR device got connected/disconnected
        private void OnDeviceConnected(int index, bool connected)
        {

            if (connected)
            {
                if (OpenVR.System != null)
                {
                    //lets figure what type of device got connected
                    ETrackedDeviceClass deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)index);
                    if (deviceClass == ETrackedDeviceClass.Controller)
                    {
                        bControlConnected = true;
                        // Debug.Log("Controller got connected at index:" + index);
                    }
                    else if (deviceClass == ETrackedDeviceClass.GenericTracker)
                    {
                        // Debug.Log("Tracker got connected at index:" + index);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (bControlConnected == true)
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
    }
}