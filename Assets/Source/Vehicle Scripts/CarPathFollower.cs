﻿using System.Collections.Generic;
using UnityEngine;
using PathCreation;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class CarPathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 0;
    float distanceTravelled;
    private List<Transform> trafficLightList;

    private bool stopping;

    public GameObject WaypointContainer = null;

    bool accelerate = true;

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
    }

    void Update()
    {
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
            if (hit.transform.tag == "Car" && hit.distance <= 9f)
            {
                accelerate = false;
            }

            Debug.DrawRay(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, 45, 0) * Vector3.forward).normalized * hit.distance, Color.red);
        }

        //45 left
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, -45, 0) * Vector3.forward), out hit, 9f))
        {
            if (hit.transform.tag == "Car" && hit.distance <= 9f)
            {
                accelerate = false;
            }

            Debug.DrawRay(this.transform.position, transform.TransformDirection(Quaternion.Euler(0, -45, 0) * Vector3.forward).normalized * hit.distance, Color.red);
        }

        if (accelerate)
        {
            stopping = false;
            if (speed < 10f)
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
                    Vector3 currentPoint = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                    transform.position = new Vector3(currentPoint.x, this.transform.position.y, currentPoint.z);

                    Vector3 currentEulerRotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
                    transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, currentEulerRotation.y, this.transform.eulerAngles.z);
                }
                else if (pathCreator.name[1] == 'W')
                {
                    Vector3 currentPoint = pathCreator.path.GetPointAtDistance(pathCreator.path.length - distanceTravelled, EndOfPathInstruction.Stop);
                    transform.position = new Vector3(currentPoint.x, this.transform.position.y, currentPoint.z);

                    Vector3 currentEulerRotation = pathCreator.path.GetRotationAtDistance(pathCreator.path.length - distanceTravelled, endOfPathInstruction).eulerAngles;
                    transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, currentEulerRotation.y +180, this.transform.eulerAngles.z);
                }
            }
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
}