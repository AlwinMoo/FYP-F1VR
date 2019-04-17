using System.Collections.Generic;
using UnityEngine;
using PathCreation;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class CarPathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 0;
    public float maxSpeed = 10;
    float distanceTravelled;
    private List<Transform> trafficLightList;

    private bool stopping;
    private bool slowDown;

    public GameObject WaypointContainer = null;

    bool accelerate = true;

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

    void Start()
    {
        if (pathCreator == null)
        {
            pathCreator = PathManager.Instance.FindPathInt(Random.Range(0, (int)PathManager.PathArrayContainer.PathGroup.NUM_OF_VALUES), Random.Range(0, (int)PathManager.PathArray.Direction.NUM_OF_VALUES));
        }

        if (WaypointContainer != null)
        {
            ReadWaypoint();
        }

        distanceTravelled = 0f;

        // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
        pathCreator.pathUpdated += OnPathChanged;
        transform.position = new Vector3(0, 4 + 0.5f, 0);
        stopping = false;
        slowDown = false;
    }

    void Update()
    {
        AudioManagerGO = GameObject.FindGameObjectWithTag("AudioManager");
        audioManager = AudioManagerGO.GetComponent<AudioManager>();

        RaycastHit hit;

        if (WaypointContainer != null)
        {
            if (trafficLightList.Count == 0)
            {
                ReadWaypoint();
            }
            
            if (Vector3.Distance(new Vector3(trafficLightList[0].position.x, 0, trafficLightList[0].position.z), new Vector3(this.transform.position.x, 0, this.transform.position.z)) <= 10)
            {
                if (trafficLightList[0].GetComponent<BasicTrafficLight>().trafficLight == BasicTrafficLight.LIGHT_STATUS.LIGHT_GREEN)
                {
                    accelerate = true;

                    if (!trafficLightList[0].GetComponent<BasicTrafficLight>().IsSplitPoint)
                    {
                        trafficLightList.RemoveAt(0);

                        if (trafficLightList.Count == 0)
                        {
                            ReadWaypoint();
                        }
                    }
                }
                else if (trafficLightList[0].GetComponent<BasicTrafficLight>().trafficLight == BasicTrafficLight.LIGHT_STATUS.LIGHT_RED)
                {
                    accelerate = false;
                }
                else
                {
                    if (!trafficLightList[0].GetComponent<BasicTrafficLight>().IsSplitPoint)
                    {
                        trafficLightList.RemoveAt(0);

                        if (trafficLightList.Count == 0)
                        {
                            ReadWaypoint();
                        }
                    }
                }
            }
            else
            {
                accelerate = true;
            }
        }

        //forward
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, 18f))
        {
            if (hit.transform.tag == "Car" && hit.distance <= 18f)
            {
                accelerate = false;
            }

            Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.forward).normalized * hit.distance, Color.red);
        }

        //45 right
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, 45, 0) * Vector3.forward), out hit, 9f))
        {
            if (hit.transform.tag == "Car" && hit.distance <= 5f)
            {
                accelerate = false;
            }
            else if (hit.transform.tag == "Car" && hit.distance <= 9f)
            {
                slowDown = true;
            }
            else
                slowDown = false;

            Debug.DrawRay(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, 45, 0) * Vector3.forward).normalized * hit.distance, Color.red);
        }

        //45 left
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, -45, 0) * Vector3.forward), out hit, 9f))
        {
            if (hit.transform.tag == "Car" && hit.distance <= 5f)
            {
                accelerate = false;
            }
            else if (hit.transform.tag == "Car" && hit.distance <= 9f)
            {
                slowDown = true;
            }
            else
                slowDown = false;

            Debug.DrawRay(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, -45, 0) * Vector3.forward).normalized * hit.distance, Color.red);
        }

        if (slowDown)
            maxSpeed = 5;
        else
            maxSpeed = 10;

        if (accelerate)
        {
            stopping = false;
            if (speed < maxSpeed)
            {
                speed += 8f * Time.deltaTime;
            }
        }
        else
        {
            stopping = true;

            if (speed > 0f)
            {
                speed -= 15f * Time.deltaTime;
            }
            else if (speed < 0f)
            {
                speed = 0f;
            }
        }

        if (pathCreator != null || stopping)
        {
            if (pathCreator.name[0] == 'C')
            {

                distanceTravelled += speed * Time.deltaTime;

                if (pathCreator.name[1] == 'C') //CCW
                {
                    Vector3 currentPoint = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop);
                    transform.position = new Vector3(currentPoint.x, this.transform.position.y, currentPoint.z);

                    Vector3 currentEulerRotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
                    transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, currentEulerRotation.y, this.transform.eulerAngles.z);
                }
                else if (pathCreator.name[1] == 'W')
                {
                    Vector3 currentPoint = pathCreator.path.GetPointAtDistance(pathCreator.path.length - distanceTravelled, EndOfPathInstruction.Loop);
                    transform.position = new Vector3(currentPoint.x, this.transform.position.y, currentPoint.z);

                    Vector3 currentEulerRotation = pathCreator.path.GetRotationAtDistance(pathCreator.path.length - distanceTravelled, endOfPathInstruction).eulerAngles;
                    transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, currentEulerRotation.y +180, this.transform.eulerAngles.z);
                }
            }
        }

        if (!source.isPlaying)
        {
            source.Play();
        }

        float distance = Vector3.Distance(prevPosition, transform.position);

        if (!soundSwap)
        {
            if (soundStartTime + 1 < Time.time)
            {
                soundSwap = true;
            }
        }

        if (this.GetComponent<CarPathFollower>().speed < 0)
        {
            if (soundSwap)
            {
                //should replace with idle sound
                source.Stop();

                prevDistance = distance;
                prevPosition = transform.position;
            }
        }
        else if ((distance + 0.8) < prevDistance)
        {
            if (source.clip == null || soundSwap)
            {
                source.pitch = 1.0f;
                source.loop = false;
                source.volume = audioManager.GetMasterVolume();

                soundStartTime = Time.time;
                soundSwap = false;
                prevDistance = distance;
                prevPosition = transform.position;

                audioClip = brakeSound;
            }
        }
        else
        {
            if (source.clip == null || soundSwap)
            {
                source.pitch = 1.0f;
                source.loop = true;
                source.volume = audioManager.GetMasterVolume();

                soundStartTime = Time.time;
                soundSwap = false;
                prevDistance = distance;
                prevPosition = transform.position;

                audioClip = gasSound;
            }
        }

        if (!soundSwap)
        {
            PlayAudio(audioClip);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    void ReadWaypoint()
    {
        trafficLightList = new List<Transform>();

        foreach (Transform child in WaypointContainer.transform)
        {
            trafficLightList.Add(child);
        }
    }

    Transform NearestTrafficLight()
    {
        float newDistance = Mathf.Infinity;
        float currDistance;
        Transform nearestTrafficLight = null;

        for (int i = 0; i < trafficLightList.Count; ++i)
        {
            currDistance = Vector3.Distance(trafficLightList[i].position, this.transform.position);

            if (currDistance < newDistance /*&& Approximation(Vector3.Dot((thatOmittedY - thisOmittedY).normalized, this.transform.TransformDirection(Vector3.forward)), 1, 0.01f)*/)
            {
                newDistance = currDistance;
                nearestTrafficLight = trafficLightList[i];
            }
        }

        return nearestTrafficLight;
    }

    private bool Approximation(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) < tolerance);
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
        source.Play();
    }
}