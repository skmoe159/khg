using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject fencePrefab; // 장애물 프리팹을 인스펙터에서 설정할 수 있도록 SerializeField로 선언
    [SerializeField] private GameObject applePrefab; // 사과 프리팹을 인스펙터에서 설정할 수 있도록 SerializeField로 선언
    [SerializeField] private GameObject coinPrefab; // 코인 프리팹을 인스펙터에서 설정할 수 있도록 SerializeField로 선언
    [SerializeField] private float spawnChance = .3f; // 장애물과 사과가 생성될 확률을 설정할 수 있도록 SerializeField로 선언 (예시로 0.5f를 사용)
    [SerializeField] private float coinSpawnChance = .5f; // 사과가 생성될 확률을 설정할 수 있도록 SerializeField로 선언 (예시로 0.5f를 사용)
    [SerializeField] private float coinSpawnInterval = 2.0f; // 코인이 생성될 간격을 설정할 수 있도록 SerializeField로 선언 (예시로 2.5f를 사용)
    [SerializeField] private float[] fenceSpawnLength = { -2.5f, 0f, 2.5f }; // 장애물의 스폰 위치를 설정할 수 있도록 float 배열로 선언 (예시로 -2.5, 0, 2.5를 사용)

    List<int> fences = new List<int>() { 0, 1, 2 }; // 장애물의 위치 인덱스를 관리하기 위한 리스트 생성 (0, 1, 2는 fenceSpawnLength 배열의 인덱스에 해당)
    void Start()
    {
        SpawnFences(); // Start 메서드에서 SpawnFence 메서드를 호출하여 장애물을 생성
        SpawnApple(); // Start 메서드에서 SpawnApple 메서드를 호출하여 사과를 생성
        SpawnCoin(); // Start 메서드에서 SpawnCoin 메서드를 호출하여 코인을 생성
    }

    // fenceSpawnLength[selectedLane]해당 코드로 인해
    // List에 0, 1, 2는 fenceSpawnLength 배열의 인덱스에 해당하며, 
    // 각각의 인덱스는 장애물이 생성될 위치를 나타냄 (예시로 0 = -2.5, 1 = 0, 2 = 2.5)
    // 랜덤 List에서 2가 할당될 경우 2.5f 위치에서 장애물이 생성되며, 
    // fences.RemoveAt(randomLaneIndex)로 해당 값 제거 = 2.5f 위치에 장애물 생성 방지
    // 이 과정을 반복함으로써 장애물이 중복 생성되는 것을 방지, 각 장애물이 고유한 위치에 생성되도록 보장
    void SpawnFences()
    {
        int randomFenceCount = Random.Range(0, fenceSpawnLength.Length); // randomFenceCount를 0과 3 사이에서 랜덤으로 선택하여 생성할 장애물의 개수를 결정 (0, 1, 2 중 하나)
        
        for (int i = 0; i < randomFenceCount; i++) // i가 randomFenceCount보다 작을 시 i를 1씩 증가시키며 반복
        {
            if (fences.Count <= 0) break; // fences 리스트가 비어있을 때 반복문 종료

            int selectedLane = SelectLane();

            // spawnPosition을 새로운 Vector3로 설정 (fenceSpawnLength[selectedLane], 현재 오브젝트의 y 위치 - 0.2f, 현재 오브젝트의 z 위치)
            Vector3 spawnPosition = new Vector3(fenceSpawnLength[selectedLane], transform.position.y, transform.position.z);
            // 새로운 장애물 게임 오브젝트를 생성하여 Instantiate 메서드로 생성 (fencePrefab, spawnPosition 위치, 회전 없음, 현재 오브젝트를 부모로 설정)
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }


    void SpawnApple()
    {
        if (Random.value > spawnChance || fences.Count <= 0) return; // spawnChance 확률로 사과 생성 (예: 0.3f이면 30% 확률로 사과 생성)
        

        int selectedAppleLane = SelectLane(); // 사과를 생성할 랜덤한 장애물 위치를 선택

        Vector3 spawnPosition = new Vector3(fenceSpawnLength[selectedAppleLane], transform.position.y, transform.position.z);
        Instantiate(applePrefab, spawnPosition, Quaternion.identity, this.transform);

    }
    void SpawnCoin()
    {
        if (Random.value > coinSpawnChance || fences.Count <= 0) return; // coinSpawnChance 확률로 코인 생성 (예: 0.5f이면 50% 확률로 코인 생성)

        int selectedLane = SelectLane(); // 코인을 생성할 랜덤한 장애물 위치를 선택

        int maxRange = 6;
        int randomCoinCount = Random.Range(1, maxRange); // randomCoinCount를 1과 maxRange 사이에서 랜덤으로 선택하여 생성할 코인의 개수를 결정 (예: 1과 5 사이에서 랜덤으로 선택)

        float topOfChunkZPos = transform.position.z + (coinSpawnInterval * 2f); // topOfChunkZPos를 현재 오브젝트의 z 위치에 coinSpawnInterval의 2배를 더한 값으로 설정 (예: 현재 z 위치 + 4f)
        for (int i = 0; i < randomCoinCount; i++)
        {
            float spawnPositionZ = topOfChunkZPos - (i * coinSpawnInterval); // spawnPositionZ를 topOfChunkZPos에 i와 coinSpawnInterval의 곱을 더한 값으로 설정 (예: topOfChunkZPos + (0, 1, 2, ...) * coinSpawnInterval)
            Vector3 spawnPosition = new Vector3(fenceSpawnLength[selectedLane], transform.position.y, spawnPositionZ); // spawnPosition을 새로운 Vector3로 설정 (fenceSpawnLength[selectedLane], 현재 오브젝트의 y 위치, spawnPositionZ)
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform);

        }
    }
    
    private int SelectLane()
    {
        int randomLaneIndex = Random.Range(0, fences.Count); // randomLaneIndex를 0과 fences 리스트의 길이 사이에서 랜덤으로 선택
        int selectedLane = fences[randomLaneIndex]; // selectedLane을 fences 리스트에서 randomLaneIndex에 해당하는 요소로 설정
        fences.RemoveAt(randomLaneIndex); // 생성된 장애물의 인덱스를 리스트에서 제거하여 중복 생성 방지
        return selectedLane;
    }
}
