using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class OnOffCar : MonoBehaviour
    {
        public bool bOnOff = false;

        bool bOnce = false;
        public void OnButtonDown()
        {
            if (bOnce == false)
            {
                if (bOnOff == false)
                {
                    bOnOff = true;
                }
                else
                {
                    bOnOff = false;
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