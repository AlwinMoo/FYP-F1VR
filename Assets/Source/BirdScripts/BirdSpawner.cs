using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    private BirdClass crow;
    private BirdClass robin;
    private BirdClass sparrow;

    public int crowSpawnMin;
    private int currentCrowAmt;

    public int robinSpawnMin;
    private int currentRobinAmt;

    public int sparrowSpawnMin;
    private int currentSparrowAmt;

    public int birdSpawnMax;
    private int currentBirdAmt;

    private GameObject player;
    private Vector3 playerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("ChanceToSpawnBird", 5.0f, 5.0f);//!< delay for ChanceToSpawnBird */
    }

    void Update()
    {
        playerPosition = player.transform.position;
        SpawnBird();
    }

    /// <summary>
    /// Spawns birds at the start of the game
    /// </summary>
    public void SpawnBird()
    {
        if (crowSpawnMin > currentCrowAmt && currentBirdAmt < birdSpawnMax)
        {
            Vector3 temp = new Vector3(Random.Range(-20, 20) + playerPosition.x, 5 + playerPosition.y, Random.Range(-20, 20) + playerPosition.z);
            crow = BirdObjectPool.Instance.SpawnFromPool("crow");
            crow.transform.localScale *= 3;
            crow.transform.position = temp;
            currentCrowAmt++;
            currentBirdAmt++;
        }
        if (robinSpawnMin > currentRobinAmt && currentBirdAmt < birdSpawnMax)
        {
            Vector3 temp = new Vector3(Random.Range(-20, 20) + playerPosition.x, 5 + playerPosition.y, Random.Range(-20, 20) + playerPosition.z);
            robin = BirdObjectPool.Instance.SpawnFromPool("robin");
            robin.transform.localScale *= 3;
            robin.transform.position = temp;
            currentRobinAmt++;
            currentBirdAmt++;
        }
        if (sparrowSpawnMin > currentSparrowAmt && currentBirdAmt < birdSpawnMax)
        {
            Vector3 temp = new Vector3(Random.Range(-20, 20) + playerPosition.x, 5 + playerPosition.y, Random.Range(-20, 20) + playerPosition.z);
            sparrow = BirdObjectPool.Instance.SpawnFromPool("sparrow");
            sparrow.transform.localScale *= 3;
            sparrow.transform.position = temp;
            currentSparrowAmt++;
            currentBirdAmt++;
        }
    }

    /// <summary>
    /// chooses a random bird type to spawn after a delay
    /// </summary>
    public void ChanceToSpawnBird()
    {
        if (currentBirdAmt < birdSpawnMax)
        {
            int random = Random.Range(0, 3);

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
                default:
                    break;
            }
        }
    }
}
