using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public BirdClass Prefab;
        public int size;
    }

    #region
    public static BirdObjectPool Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;//!< list of pools from Pool Class*/

    public Dictionary<string, Queue<BirdClass>> poolDictionary;//!< stores all the pools from Pool Class */

    private Queue<BirdClass> objectPool;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<BirdClass>>();

        foreach (Pool pool in pools)
        {
            objectPool = new Queue<BirdClass>();

            for (int i = 0; i < pool.size; ++i)
            {
                BirdClass obj = Instantiate(pool.Prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);//!< add it to the end of the queue */
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// pulls out the first element in the queue, sets it to active then add it to the end of the queue
    /// </summary>
    /// <param name="tag">tag found in objectpool</param>
    /// <returns></returns>
    public BirdClass SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag does not exist");
        }
        BirdClass objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    /// <summary>
    /// Sets all gameobjects in the object pools to false
    /// </summary>
    public void setAllFalse()
    {
        foreach (Pool pool in pools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                foreach (BirdClass obj in poolDictionary[pools[i].tag])
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}
