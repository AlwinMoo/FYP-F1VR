using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    float vehTimer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        vehTimer += Time.deltaTime;

        if (vehTimer > 7.0f)
        {
            foreach (var listIndex in PathManager.Instance.PathList) //search through containers
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(listIndex.PathArrayList[0].Path.path.GetPoint(0));
                if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1) //if within viewport
                    continue; //dont spawn when can see spawn point

                int ranDirection = Random.Range(0, 2);
                var newGO = ObjectPooler.Instance.SpawnFromPool("Car", Vector3.zero, Quaternion.identity, false);

                if (newGO != null)
                {
                    newGO.GetComponent<CarPathFollower>().pathCreator = listIndex.PathArrayList[ranDirection].Path; //randomise driving direction
                    newGO.GetComponent<CarPathFollower>().WaypointContainer = listIndex.PathArrayList[ranDirection].waypointContainer;
                }

                break;
            }

            vehTimer = 0.0f;
        }

    }

}
