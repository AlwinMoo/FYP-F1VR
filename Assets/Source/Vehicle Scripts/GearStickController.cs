using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class GearStickController : MonoBehaviour
    {
        public LinearMapping linearMapping;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (linearMapping.value >= 0.9f)
            {
                transform.parent.GetComponent<VehicleBase>().bNeutral = false;
                transform.parent.GetComponent<VehicleBase>().bReverse = true;
            }
            else if (linearMapping.value <= 0.1f)
            {
                transform.parent.GetComponent<VehicleBase>().bNeutral = false;
                transform.parent.GetComponent<VehicleBase>().bReverse = false;
            }
            else
            {
                transform.parent.GetComponent<VehicleBase>().bNeutral = true;
            }
        }
    }
}
