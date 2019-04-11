using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SedanWOVR : VehicleBaseWOVR
{

    // Use this for initialization
    public void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().mass = 1500;
        mass = this.gameObject.GetComponent<Rigidbody>().mass;

        motorForce = 4000;
        steerForce = 9000;
        brakeForce = 2 * motorForce;

        fR_Wheel = GameObject.FindWithTag("FR_Collider").GetComponent<WheelCollider>();
        fL_Wheel = GameObject.FindWithTag("FL_Collider").GetComponent<WheelCollider>();
        rR_Wheel = GameObject.FindWithTag("RR_Collider").GetComponent<WheelCollider>();
        rL_Wheel = GameObject.FindWithTag("RL_Collider").GetComponent<WheelCollider>();

        fR_T = GameObject.FindWithTag("FR_Transform").GetComponent<Transform>();
        InitWheelScale(fR_T, new Vector3(1f, 1f, 1f));
        fL_T = GameObject.FindWithTag("FL_Transform").GetComponent<Transform>();
        InitWheelScale(fL_T, new Vector3(1f, 1f, 1f));
        rR_T = GameObject.FindWithTag("RR_Transform").GetComponent<Transform>();
        InitWheelScale(rR_T, new Vector3(1f, 1f, 1f));
        rL_T = GameObject.FindWithTag("RL_Transform").GetComponent<Transform>();
        InitWheelScale(rL_T, new Vector3(1f, 1f, 1f));

        driveTrain = DriveTrain.DRIVE_RWD;
        vehicleType = VehicleType.VEH_SEDAN;

        //base.OnObjectSpawn();
    }
}
