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

    public List<Pool> pools;

    public Dictionary<string, Queue<BirdClass>> poolDictionary;

    private Queue<BirdClass> objectPool;

    // Start is called before the first frame update
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
                objectPool.Enqueue(obj);//add it to the end of the queue
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public BirdClass SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag does not exist");
        }
        BirdClass objectToSpawn = poolDictionary[tag].Dequeue();//pull out first element in the queue
        objectToSpawn.gameObject.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);//add it to the end of the queue

        return objectToSpawn;
    }
    
    //Returns the list of gameobjects
    public Queue<BirdClass> GetPoolList()
    {
        return objectPool;
    }

    public void setAllFalse()
    {
        foreach (BirdClass obj in objectPool)
        {
            obj.gameObject.SetActive(false);
        }
        foreach (BirdClass obj in poolDictionary[pools[0].tag])
        {
            obj.gameObject.SetActive(false);
        }
    }
}
