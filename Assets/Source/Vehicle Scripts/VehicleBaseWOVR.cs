using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(AudioSource))]
public class VehicleBaseWOVR : MonoBehaviour, IPooledObject
{
    #region Elastic collision
    //public Vector3 u1 { get; set; }
    //public Vector3 u2 { get; set; }
    //public Vector3 v1 { get; set; }
    //public Vector3 v2 { get; set; }
    //public float m1 { get; set; }
    //public float m2 { get; set; }
    #endregion

    public float mass { get; set; }

    #region Car control
    public float m_horizonetalInput { get; set; }
    public float m_verticalInput { get; set; }
    public float m_steeringAngle { get; set; }
    #endregion

    #region Car force
    public float motorForce { get; set; }
    public float steerForce { get; set; }
    public float brakeForce { get; set; }
    #endregion

    #region Wheel colliders
    public WheelCollider fR_Wheel { get; set; }
    public WheelCollider fL_Wheel { get; set; }
    public WheelCollider rR_Wheel { get; set; }
    public WheelCollider rL_Wheel { get; set; }
    public Transform fR_T { get; set; }
    public Transform fL_T { get; set; }
    public Transform rR_T { get; set; }
    public Transform rL_T { get; set; }
    #endregion

    //private NavMeshAgent agent;

    #region Car audio
    //AudioManager audioManager;
    //public AudioSource source;

    //private Vector3 prevPosition;
    //private float prevDistance;
    //private float soundStartTime;
    //private bool soundSwap;
    //private AudioClip audioClip;
    //public AudioClip gasSound;
    //public AudioClip brakeSound;
    //public AudioClip idleSound;
    #endregion

    float maxSteerAngle = 30;

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
    public virtual void OnObjectSpawn()
    {
        //audioManager = AudioManager.instance;
        //prevPosition = transform.position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // If the gameobject is not owned by the client
        rR_Wheel.motorTorque = 0;
        rL_Wheel.motorTorque = 0;

        fR_Wheel.motorTorque = 0;
        fL_Wheel.motorTorque = 0;

        if (Input.GetKey(KeyCode.Space))
        {
            rL_Wheel.brakeTorque = brakeForce;
            rR_Wheel.brakeTorque = brakeForce;
            fL_Wheel.brakeTorque = brakeForce;
            fR_Wheel.brakeTorque = brakeForce;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rL_Wheel.brakeTorque = 0;
            rR_Wheel.brakeTorque = 0;
            fL_Wheel.brakeTorque = 0;
            fR_Wheel.brakeTorque = 0;
        }

        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < -3 && Input.GetKeyDown(KeyCode.W))
        {
            rL_Wheel.brakeTorque = brakeForce;
            rR_Wheel.brakeTorque = brakeForce;
            fL_Wheel.brakeTorque = brakeForce;
            fR_Wheel.brakeTorque = brakeForce;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            rL_Wheel.brakeTorque = 0;
            rR_Wheel.brakeTorque = 0;
            fL_Wheel.brakeTorque = 0;
            fR_Wheel.brakeTorque = 0;
        }

        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 3 && Input.GetKeyDown(KeyCode.S))
        {
            rL_Wheel.brakeTorque = brakeForce;
            rR_Wheel.brakeTorque = brakeForce;
            fL_Wheel.brakeTorque = brakeForce;
            fR_Wheel.brakeTorque = brakeForce;
        }
        else if (Input.GetKeyUp(KeyCode.S))
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

        //if (!source.isPlaying)
        //{
        //    source.Play();
        //}

        //float distance = Vector3.Distance(prevPosition, transform.position);

        //if (!soundSwap)
        //{
        //    if (soundStartTime + 1 < Time.time)
        //    {
        //        soundSwap = true;
        //    }
        //}

        ////if (this.GetComponent<CarPathFollower>().speed == 0)
        ////{
        ////    //should replace with idle sounsd
        ////    source.Stop();

        ////    prevDistance = distance;
        ////    prevPosition = transform.position;
        ////}
        //if ((distance + 0.8) < prevDistance)
        //{
        //    if (source.clip == null || soundSwap)
        //    {
        //        source.pitch = 1.0f;
        //        source.loop = false;
        //        source.volume = audioManager.GetSFXVolume();

        //        soundStartTime = Time.time;
        //        soundSwap = false;
        //        prevDistance = distance;
        //        prevPosition = transform.position;

        //        audioClip = brakeSound;
        //    }
        //}
        //else
        //{
        //    if (source.clip == null || soundSwap)
        //    {
        //        source.pitch = 1.0f;
        //        source.loop = true;
        //        source.volume = audioManager.GetSFXVolume();

        //        soundStartTime = Time.time;
        //        soundSwap = false;
        //        prevDistance = distance;
        //        prevPosition = transform.position;

        //        audioClip = gasSound;
        //    }
        //}

        //if (!soundSwap)
        //{
        //    PlayAudio(audioClip);
        //}

    }

    public virtual void GetInput()
    {
        m_verticalInput = Input.GetAxis("Vertical");
        m_horizonetalInput = Input.GetAxis("Horizontal");

        //if (grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        //{
        //    m_verticalInput = 1;
        //}
        //else if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.RightHand))
        //{
        //    m_verticalInput = 0;
        //}

        //m_horizonetalInput = GameObject.FindGameObjectWithTag("SteeringWheel").transform.eulerAngles.z;
        //m_horizonetalInput = (m_horizonetalInput > 180) ? m_horizonetalInput - 360 : m_horizonetalInput;
        //m_horizonetalInput /= -180;
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

        GetComponent<VehicleBaseWOVR>().enabled = true;
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

    public void PlayAudio(AudioClip music)
    {
        //if (source.clip != null)
        //{
        //    if (source.clip.name == music.name)
        //        return;
        //}

        ////changing music it plays
        //source.Stop();
        //source.clip = music;
        //source.Play();
    }
}
