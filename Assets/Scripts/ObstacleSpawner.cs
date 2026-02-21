using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs; // 장애물 프리팹 배열을 인스펙터에서 설정할 수 있도록 SerializeField로 선언
    [SerializeField] private int maxObstacles = 10;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private Transform obstacleParent; // 생성된 장애물의 부모 오브젝트를 설정할 수 있도록 Transform 변수 선언

    List<GameObject> obstacles = new List<GameObject>(); // 장애물을 관리하기 위한 리스트 생성

    void Start()
    {
        StartCoroutine(SpawnObstaclesOverTime()); // 장애물을 일정 시간 간격으로 생성하는 코루틴 시작
    }

    IEnumerator SpawnObstaclesOverTime()
    {
        while (true) // 무한 루프를 사용하여 계속해서 장애물을 생성
        {
            
            yield return new WaitForSeconds(spawnInterval); // 장애물 생성 간격을 spawnInterval로 설정
            SpawnObstacle();
            if (obstacles.Count >= maxObstacles) // 장애물의 개수가 maxObstacles보다 많을 때
            {
                RemoveObstacle(); // 리스트의 첫 번째 장애물을 제거하여 관리
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle()
    {
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]; // 장애물 프리팹 중에서 랜덤으로 선택
        Vector3 spawnPosition = new Vector3(Random.Range(-3f, 3f), transform.position.y, transform.position.z); // 장애물의 스폰 위치를 랜덤으로 설정
        // 새로운 장애물 게임 오브젝트를 생성하여 obstacle 변수에 할당 (obstaclePrefab, spawnPosition 위치, 랜덤 회전)
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);

        obstacles.Add(obstacle); // 생성된 장애물을 리스트에 추가하여 관리
    }

    void RemoveObstacle()
    {
        obstacles.RemoveAt(0); // 장애물을 리스트에서 제거
        Destroy(obstacles[0]); // 장애물 게임 오브젝트를 파괴
    }
}