using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class OnOffCar : MonoBehaviour
    {
        public GameObject Normal;
        public GameObject Green;
        public bool bOnOff = false;

        bool bOnce = false;
        public void OnButtonDown()
        {
            if (bOnce == false)
            {
                if (bOnOff == false)
                {
                    bOnOff = true;
                    Green.SetActive(true);
                    Normal.SetActive(false);
                }
                else
                {
                    bOnOff = false;
                    Green.SetActive(false);
                    Normal.SetActive(true);
                }
                bOnce = true;
            }
        }

        public void OnButtonUp()
        {
            bOnce = false;
        }

    }
}