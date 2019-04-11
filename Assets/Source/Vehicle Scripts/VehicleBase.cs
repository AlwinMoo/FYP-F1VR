using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Valve.VR.InteractionSystem
{
    public class VehicleBase : MonoBehaviour
    {

        //public Vector3 u1 { get; set; }
        //public Vector3 u2 { get; set; }
        //public Vector3 v1 { get; set; }
        //public Vector3 v2 { get; set; }
        //public float m1 { get; set; }
        //public float m2 { get; set; }


        public float mass { get; set; }

        public float m_horizonetalInput { get; set; }
        public float m_verticalInput { get; set; }
        public float m_steeringAngle { get; set; }

        public float motorForce { get; set; }
        public float steerForce { get; set; }
        public float brakeForce { get; set; }

        public WheelCollider fR_Wheel { get; set; }
        public WheelCollider fL_Wheel { get; set; }
        public WheelCollider rR_Wheel { get; set; }
        public WheelCollider rL_Wheel { get; set; }
        public Transform fR_T { get; set; }
        public Transform fL_T { get; set; }
        public Transform rR_T { get; set; }
        public Transform rL_T { get; set; }

        //GameObject aimingRay;
        //static Material lineMaterial;
        float maxSteerAngle = 30;
        //bool cancelShoot;

        public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch"); // Button pushed

        public enum DriveTrain
        {
            DRIVE_AWD,
            DRIVE_RWD,
            DRIVE_FWD,
        }
        public DriveTrain driveTrain { get; set; }

        public enum VehicleType
        {
            VEH_SEDAN,
            VEH_VAN,
            VEH_MONSTER_TRUCK,
        }
        public VehicleType vehicleType { get; set; }

        public Vector3 parentDir;

        // Use this for initialization
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {
            // If the gameobject is not owned by the client
            rR_Wheel.motorTorque = 0;
            rL_Wheel.motorTorque = 0;

            fR_Wheel.motorTorque = 0;
            fL_Wheel.motorTorque = 0;

            //if (Input.GetMouseButton(0) && (this.gameObject.GetComponent(typeof(Collider)) as Collider) != false)
            //{
            //    //RaycastHit hit;
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    Plane plane = new Plane(Vector3.up, this.transform.position);
            //    float distToPlane;

            //    if (plane.Raycast(ray, out distToPlane))
            //    {
            //        Vector3 hitPos = ray.GetPoint(distToPlane);

            //        if (!aimingRay)
            //            aimingRay = new GameObject();

            //        aimingRay.transform.position = this.transform.position;

            //        //CreateLineMaterial();
            //        if (!aimingRay.GetComponent<LineRenderer>())
            //        {
            //            aimingRay.AddComponent<LineRenderer>();
            //        }

            //        LineRenderer aimLine = aimingRay.GetComponent<LineRenderer>();
            //        aimLine.material = new Material(Shader.Find("Sprites/Default"));

            //        Color endRed = Color.red;
            //        endRed.a = 0.3f;
            //        Color startRed = Color.red;
            //        startRed.a = 0.6f;
            //        aimLine.startColor = startRed;
            //        aimLine.endColor = endRed;
            //        aimLine.startWidth = 0.15f;
            //        aimLine.endWidth = aimLine.startWidth;

            //        aimLine.SetPosition(0, transform.position);
            //        aimLine.SetPosition(1, hitPos);

            //        Vector3 dir = hitPos - transform.position;
            //        dir.y = 0;

            //        parentDir = dir;


            //        cancelShoot = true; 
            //    }
            //}

            //if (Input.GetMouseButtonUp(0) && cancelShoot != false)
            //{
            //    cancelShoot = false;
            //    GameObject.Destroy(aimingRay);
            //}

            if (grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                rL_Wheel.brakeTorque = brakeForce;
                rR_Wheel.brakeTorque = brakeForce;
                fL_Wheel.brakeTorque = brakeForce;
                fR_Wheel.brakeTorque = brakeForce;
            }
            if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.LeftHand))
            {
                rL_Wheel.brakeTorque = 0;
                rR_Wheel.brakeTorque = 0;
                fL_Wheel.brakeTorque = 0;
                fR_Wheel.brakeTorque = 0;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }

        }
        public virtual void GetInput()
        {
            //m_verticalInput = Input.GetAxis("Vertical");
            //m_horizonetalInput = Input.GetAxis("Horizontal");

            if (grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                m_verticalInput = 1;
            }
            else if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.RightHand))
            {
                m_verticalInput = 0;
            }

            m_horizonetalInput = GameObject.FindGameObjectWithTag("SteeringWheel").transform.eulerAngles.z;
            m_horizonetalInput = (m_horizonetalInput > 180) ? m_horizonetalInput - 360 : m_horizonetalInput;
            m_horizonetalInput /= -180;
        }
        public virtual void Steer()
        {
            m_steeringAngle = maxSteerAngle * m_horizonetalInput;
            // Update the steer angles for the wheels
            fL_Wheel.steerAngle = m_steeringAngle;
            fR_Wheel.steerAngle = m_steeringAngle;
        }

        public virtual void SetComponentActive(bool _status)
        {
            ///Set children
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(_status);
            }

            ///Set monobehavior components
            MonoBehaviour[] component = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in component)
            {
                c.enabled = _status;
            }

            foreach (var c in this.gameObject.GetComponents(typeof(Collider)))
            {
                (c as Collider).enabled = _status;
            }

            (this.gameObject.GetComponent(typeof(Renderer)) as Renderer).enabled = _status;

            GetComponent<VehicleBase>().enabled = true;
        }

        public virtual void Accelerate()
        {
            // Simulate all wheel drive
            switch (driveTrain)
            {
                case DriveTrain.DRIVE_AWD:
                    {
                        fL_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        fR_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        rL_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        rR_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        break;
                    }
                case DriveTrain.DRIVE_RWD:
                    {
                        rL_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        rR_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        break;
                    }
                case DriveTrain.DRIVE_FWD:
                    {
                        fL_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        fR_Wheel.motorTorque = m_verticalInput * motorForce / (mass * 0.001f);
                        break;
                    }
            }

        }
        public virtual void UpdateWheelPoses()
        {
            // TODO: STEERING WHEEL POS UPDATES HERE
            UpdateWheelPose(fR_Wheel, fR_T);
            UpdateWheelPose(fL_Wheel, fL_T);
            UpdateWheelPose(rR_Wheel, rR_T);
            UpdateWheelPose(rL_Wheel, rL_T);
        }

        public virtual void InitWheelScale(Transform _transform, Vector3 _wheelScale)
        {
            _transform.localScale = _wheelScale;
        }

        public virtual void UpdateWheelPose(WheelCollider _collider, Transform _transform)
        {
            Vector3 _pos = _transform.position;
            Quaternion _quat = _transform.rotation;

            _collider.GetWorldPose(out _pos, out _quat);
            _transform.position = _pos;
            _transform.rotation = _quat;
            //_transform.localScale = new Vector3(0.3f, 0.6f, 0.6f);
        }

        public virtual void FixedUpdate()
        {
            GetInput();
            Steer();
            Accelerate();
            UpdateWheelPoses();

            AnalogueSpeedoMeter.SpeedToAngle(this.GetComponent<Rigidbody>().velocity.magnitude, 0, 25);
            this.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(this.GetComponent<Rigidbody>().velocity, 5);
        }

        /// <summary>
        /// Receives damage when the player enters collision with an enemy. Different vehicle types take different damage amounts
        /// </summary>
        public virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
        /// <summary>
        /// Triggers fire when a client shoots
        /// </summary>
        //  public override void triggerShoot(RpcArgs args)
        //  {
        //      int ShooterID = args.GetNext<int>();
        //      bool ShootingStatus = args.GetNext<bool>();

        //      if (this.gameObject.tag != "Player" + ShooterID)
        //      {
        //	return;
        //}

        //      switch (this.gameObject.GetComponent<VehicleBase>().vehicleType)
        //      {
        //          case VehicleType.VEH_SEDAN:
        //              {
        //                  EventManager.TriggerEvent("MGShoot", this.gameObject.tag);
        //                  break;
        //              }
        //          case VehicleType.VEH_VAN:
        //              {
        //                  if (!ShootingStatus)
        //                  {
        //                      EventManager.TriggerEvent("CancelFire", this.gameObject.tag);
        //                      break;
        //                  }

        //                  EventManager.TriggerEvent("FireShoot", this.gameObject.tag);
        //                  break;
        //              }
        //          case VehicleType.VEH_MONSTER_TRUCK:
        //              {
        //                  EventManager.TriggerEvent("RocketShoot", this.gameObject.tag);
        //                  break;
        //              }
        //          default:
        //              break;
        //      }

        //  }

        //  public override void SendTag(RpcArgs args)
        //  {
        //      string SetName = args.GetNext<string>();
        //Debug.Log ("Found name of: " + SetName + ". Checking with " + this.gameObject.tag);
        //      if (this.gameObject.tag == "Player")
        //      {
        //          //if (GameObject.FindGameObjectWithTag(SetName) != null)
        //          //    return;
        //	Debug.Log (this.gameObject.tag + " set to " + SetName);

        //          this.gameObject.tag = SetName;
        //      }
        //  }
    }
}
