using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefabs;
    [SerializeField] private int maxObstacles = 10;
    [SerializeField] private float spawnInterval = 1f;
    private int obstacleSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnObstaclesOverTime());
    }

    IEnumerator SpawnObstaclesOverTime()
    {
        while (obstacleSpawned < maxObstacles)
        {
            SpawnObstacle();
            obstacleSpawned++;
            yield return new WaitForSeconds(spawnInterval); // 장애물 생성 간격을 spawnInterval로 설정
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-3f, 3f), 5f, transform.position.z); // 장애물의 스폰 위치를 랜덤으로 설정
        Instantiate(obstaclePrefabs, spawnPosition, Random.rotation); // 장애물을 생성
    }
}