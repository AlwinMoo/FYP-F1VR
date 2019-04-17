using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class GearStickController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 angle = this.transform.eulerAngles;

            if (angle.x < 0)
                angle.x = 0;
            else if (angle.x > 15)
                angle.x = 15;

            this.transform.eulerAngles = angle;
        }

        public void MaxAngle()
        {
            transform.parent.GetComponent<VehicleBase>().bReverse = false;
        }

        public void MinAngle()
        {
            transform.parent.GetComponent<VehicleBase>().bReverse = true;
        }
    }
}
