using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathManager : MonoBehaviour
{
    [System.Serializable]
    public struct PathArray
    {
        public enum Direction
        {
            CCW,
            CW,
            NUM_OF_VALUES,
        }
        public Direction pathDirection;
        public PathCreator Path;

        [Tooltip("Ensure that waypoints in the container are placed in order")]
        public GameObject waypointContainer;
    }

    [System.Serializable]
    public struct PathArrayContainer
    {
        public enum PathGroup
        {
            D1,
            NUM_OF_VALUES,
        }
        public PathGroup pathGroup;
        public PathArray[] PathArrayList;
    }
    
    public PathArrayContainer[] PathList;

    #region Singleton

    public static PathManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PathCreator FindPathEnum(PathArrayContainer.PathGroup ContainerToSearch, PathArray.Direction DirectionOfRoad)
    {
        PathArray[] tempList = null;

        foreach(var cont in PathList)
        {
            if (cont.pathGroup != ContainerToSearch)
                continue;
            else
                tempList = cont.PathArrayList;
        }

        if (tempList != null)
        {
            foreach (var p in tempList)
            {
                if (p.pathDirection != DirectionOfRoad)
                    continue;
                else
                    return p.Path;
            }
        }

        return null;
    }

    public PathCreator FindPathInt(int ContainerToSearch, int DirectionOfRoad)
    {
        PathArray[] tempList = null;

        foreach (var cont in PathList)
        {
            if (cont.pathGroup != (PathArrayContainer.PathGroup)ContainerToSearch)
                continue;
            else
                tempList = cont.PathArrayList;
        }

        if (tempList != null)
        {
            foreach (var p in tempList)
            {
                if (p.pathDirection != (PathArray.Direction)DirectionOfRoad)
                    continue;
                else
                    return p.Path;
            }
        }

        return null;
    }
}
