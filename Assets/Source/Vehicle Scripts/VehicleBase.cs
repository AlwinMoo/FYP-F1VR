﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Valve.VR.InteractionSystem
{
    public class VehicleBase : MonoBehaviour
    {

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

        OnOffCar _OnOffCar;
        
        bool bOnOrOff;

        [NonSerialized]
        public bool bReverse = false;

        [NonSerialized]
        public bool bNeutral = false;

        float maxSteerAngle = 30;

        public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch"); // Button pushed

        #region Car audio
        private GameObject AudioManagerGO;
        private AudioManager audioManager;
        public AudioSource source;

        private Vector3 prevPosition;
        private float prevDistance;
        private float soundStartTime;
        private bool soundSwap;
        private AudioClip audioClip;
        public AudioClip gasSound;
        public AudioClip brakeSound;
        #endregion

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

        // Use this for initialization
        public virtual void Start()
        {
            AudioManagerGO = GameObject.FindGameObjectWithTag("AudioManager");
            audioManager = AudioManagerGO.GetComponent<AudioManager>();

            GameObject go_OFButton = GameObject.FindGameObjectWithTag("OFButton");
            _OnOffCar = go_OFButton.GetComponent<OnOffCar>();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            bOnOrOff = _OnOffCar.bOnOff;

            if (bOnOrOff && !bNeutral)
            {
                // If the gameobject is not owned by the client
                rR_Wheel.motorTorque = 0;
                rL_Wheel.motorTorque = 0;

                fR_Wheel.motorTorque = 0;
                fL_Wheel.motorTorque = 0;

                if (!source.isPlaying)
                {
                    source.Play();
                }

                if (!soundSwap)
                {
                    if (soundStartTime + 1 < Time.time)
                    {
                        soundSwap = true;
                    }
                }

                if (grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    rL_Wheel.brakeTorque = brakeForce;
                    rR_Wheel.brakeTorque = brakeForce;
                    fL_Wheel.brakeTorque = brakeForce;
                    fR_Wheel.brakeTorque = brakeForce;

                    if (source.clip == null || soundSwap)
                    {
                        source.pitch = 1.0f;
                        source.loop = false;
                        source.volume = audioManager.GetMasterVolume();

                        soundStartTime = Time.time;
                        soundSwap = false;

                        audioClip = brakeSound;
                    }
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

                if ((!grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) && (!grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand)))
                {
                    if (soundSwap)
                    {
                        //should replace with idle sound
                        source.Stop();
                    }
                }

                if (!soundSwap)
                {
                    PlayAudio(audioClip);
                }
            }
        }
        public virtual void GetInput()
        {
            //m_verticalInput = Input.GetAxis("Vertical");
            //m_horizonetalInput = Input.GetAxis("Horizontal");

            if (bOnOrOff || !bNeutral)
            {
                if (grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    if (bReverse == false)
                        m_verticalInput = 1;
                    else
                        m_verticalInput = -1;

                    if (source.clip == null || soundSwap)
                    {
                        source.pitch = 1.0f;
                        source.loop = true;
                        source.volume = audioManager.GetMasterVolume();

                        soundStartTime = Time.time;
                        soundSwap = false;

                        audioClip = gasSound;
                    }
                }
                else if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.RightHand))
                {
                    m_verticalInput = 0;
                }

                m_horizonetalInput = GameObject.FindGameObjectWithTag("SteeringWheel").transform.eulerAngles.z;
                m_horizonetalInput = (m_horizonetalInput > 180) ? m_horizonetalInput - 360 : m_horizonetalInput;
                m_horizonetalInput /= -180;
            }
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
            this.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(this.GetComponent<Rigidbody>().velocity, 10);
        }
        
        public virtual void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer != 10) return;

            if (collision.gameObject.CompareTag("Car"))
            {
                collision.gameObject.GetComponent<CarPathFollower>().playerFound = true;
                collision.gameObject.GetComponent<CarPathFollower>().accelerate = false;
            }
        }

        public virtual void OnTriggerLeave(Collider collision)
        {
            if (collision.gameObject.layer != 10) return;

            if (collision.gameObject.CompareTag("Car"))
            {
                collision.gameObject.GetComponent<CarPathFollower>().playerFound = false;
                collision.gameObject.GetComponent<CarPathFollower>().accelerate = true;
            }
        }

        public void PlayAudio(AudioClip music)
        {
            if (source.clip != null)
            {
                if (source.clip.name == music.name)
                    return;
            }

            //changing music it plays
            source.Stop();
            source.clip = music;
            source.volume = audioManager.GetMasterVolume();
            source.Play();
        }
    }
}
