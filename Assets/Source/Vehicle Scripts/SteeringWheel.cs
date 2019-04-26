using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class SteeringWheel : MonoBehaviour
    {
        //private Transform Car;
        //private Vector3 offset;
        //private Vector3 defaultWheelAngle;

        // Start is called before the first frame update
        void Start()
        {
            //Car = GameObject.FindGameObjectWithTag("ParentWheel").transform;
            //offset = new Vector3(-0.3395319f, 0.9059856f, 0.5334544f);
        }

        // Update is called once per frame
        void Update()
        {
            //this.transform.eulerAngles = new Vector3(Car.eulerAngles.x + 15, Car.eulerAngles.y, this.transform.eulerAngles.z);
            //this.transform.position = Car.position;

            //if (!CircularDrive.isDrivingActive && !Mathf.Approximately(0.0f, this.transform.rotation.z)) //if none of the hands are holding anything
            //{
            //    float deltaAngle = 150 * Time.deltaTime;
            //    float wheelAngle = this.transform.rotation.eulerAngles.z;

            //    wheelAngle = (wheelAngle > 180) ? wheelAngle - 360 : wheelAngle;

            //    if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle))
            //        wheelAngle = 0f;
            //    else if (wheelAngle > 0f)
            //        wheelAngle -= deltaAngle;
            //    else
            //        wheelAngle += deltaAngle;

            //    this.transform.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, wheelAngle);
            //}

            Vector3 tempRotation = this.transform.localEulerAngles;

            tempRotation.z = (tempRotation.z > 180) ? tempRotation.z - 360 : tempRotation.z;
            tempRotation.z = Mathf.Clamp(tempRotation.z, -160, 160);

            this.transform.localEulerAngles = tempRotation;

            Debug.Log(tempRotation.z);
        }
    }
}
