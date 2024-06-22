using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 2.0f;
    public float obstacleHeightRange = 2.0f;
    public Vector3 spawnPosition = new Vector3(10, 0, 0);
    public float obstacleSpeed = 5.0f;

    void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObstacle()
    {
        Vector3 obstaclePosition = spawnPosition;
        obstaclePosition.y += Random.Range(-obstacleHeightRange, obstacleHeightRange);
        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
        obstacle.AddComponent<Obstacle>().speed = obstacleSpeed;
        Destroy(obstacle, 10f);
    }
}
