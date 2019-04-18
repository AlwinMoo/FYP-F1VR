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
            linearMapping = GetComponent<LinearMapping>();
        }

        // Update is called once per frame
        void Update()
        {
            if (linearMapping.value == 1)
            {
                Backward();
            }
            else if (linearMapping.value == 0)
            {
                Forward();
            }
        }

        void Forward()
        {
            transform.parent.GetComponent<VehicleBase>().bReverse = false;
        }

        void Backward()
        {
            transform.parent.GetComponent<VehicleBase>().bReverse = true;
        }
    }
}
