using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	#region Singleton

	public static ObjectPooler Instance;

	private void Awake()
	{
		Instance = this;
	}

	#endregion

	public List<Pool> Pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	// Use this for initialization
	void Start () {
		poolDictionary = new Dictionary<string, Queue<GameObject>> ();

		foreach (Pool pool in Pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();
			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate (pool.prefab);
				obj.SetActive (false);
				objectPool.Enqueue (obj);
			}

			poolDictionary.Add (pool.tag, objectPool);
		}
	}
	
	public GameObject SpawnFromPool (string _tag, Vector3 _position, Quaternion _rotation, bool dequeue = true)
	{
        if (!dequeue)
        {
            int activeCounter = 0;

            foreach (var GO in poolDictionary[_tag])
            {
                if (GO.activeInHierarchy)
                    ++activeCounter;

                if (activeCounter == poolDictionary[_tag].Count)
                    return null;
            }
        }

		if (!poolDictionary.ContainsKey (_tag)) 
		{
			Debug.LogWarning ("Pool with tag " + _tag + " doesn't exist");
			return null;
		}
		GameObject objectToSpawn = poolDictionary [_tag].Dequeue ();

		objectToSpawn.SetActive (true);
		objectToSpawn.transform.position = _position;
		objectToSpawn.transform.rotation = _rotation;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject> ();
		// If the object derives from the IpooledObject interface, call the onobjectspawn
		if (pooledObj != null) 
		{
			pooledObj.OnObjectSpawn ();
		}

		poolDictionary [_tag].Enqueue (objectToSpawn);
		Debug.Log ("new size of " + _tag + " is now " + poolDictionary [_tag].Count);
		return objectToSpawn;
	}
}
