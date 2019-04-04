using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public BirdClass crow;
    public BirdClass robin;
    public BirdClass sparrow;

    public int crowSpawnMin;
    private int currentCrowAmt;

    public int robinSpawnMin;
    private int currentRobinAmt;

    public int sparrowSpawnMin;
    private int currentSparrowAmt;

    public int birdSpawnMax;
    private int currentBirdAmt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBird();
        Debug.Log(currentBirdAmt);
        Debug.Log(currentCrowAmt);
        Debug.Log(currentRobinAmt);
        Debug.Log(currentSparrowAmt);
    }

    public void SpawnBird()
    {
        if (crowSpawnMin > currentCrowAmt && currentBirdAmt < birdSpawnMax)
        {
            Vector3 temp = new Vector3(Random.Range(-10, 10), 35, Random.Range(-10, 10));
            crow = BirdObjectPool.Instance.SpawnFromPool("crow");
            crow.transform.position = temp;
            currentCrowAmt++;
            currentBirdAmt++;
        }
        if (robinSpawnMin > currentRobinAmt && currentBirdAmt < birdSpawnMax)
        {
            Vector3 temp = new Vector3(Random.Range(-10, 10), 35, Random.Range(-10, 10));
            robin = BirdObjectPool.Instance.SpawnFromPool("robin");
            robin.transform.position = temp;
            currentRobinAmt++;
            currentBirdAmt++;
        }
        if (sparrowSpawnMin > currentSparrowAmt && currentBirdAmt < birdSpawnMax)
        {
            Vector3 temp = new Vector3(Random.Range(-10, 10), 35, Random.Range(-10, 10));
            sparrow = BirdObjectPool.Instance.SpawnFromPool("sparrow");
            sparrow.transform.position = temp;
            currentSparrowAmt++;
            currentBirdAmt++;
        }

        if (currentBirdAmt < birdSpawnMax)
        {
            int random = Random.Range(0, 100);

            switch (random)
            {
                case 0:
                {
                    if (currentCrowAmt < BirdObjectPool.Instance.pools[random].size)
                    {
                        Vector3 temp = new Vector3(Random.Range(-10, 10), 35, Random.Range(-10, 10));
                        crow = BirdObjectPool.Instance.SpawnFromPool("crow");
                        crow.transform.position = temp;
                        currentCrowAmt++;
                        currentBirdAmt++;
                    }
                    break;
                }
                case 1:
                {
                    if (currentRobinAmt < BirdObjectPool.Instance.pools[random].size)
                    {
                        Vector3 temp = new Vector3(Random.Range(-10, 10), 35, Random.Range(-10, 10));
                        robin = BirdObjectPool.Instance.SpawnFromPool("robin");
                        robin.transform.position = temp;
                        currentRobinAmt++;
                        currentBirdAmt++;
                        }
                        break;
                }
                case 2:
                {
                    if (currentSparrowAmt < BirdObjectPool.Instance.pools[random].size)
                    {
                        Vector3 temp = new Vector3(Random.Range(-10, 10), 35, Random.Range(-10, 10));
                        sparrow = BirdObjectPool.Instance.SpawnFromPool("sparrow");
                        sparrow.transform.position = temp;
                        currentSparrowAmt++;
                        currentBirdAmt++;
                    }
                    break;
                }
            }
        }
    }
}
