using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject ChunkPrefab;
    [SerializeField] private Transform chunkParent;
    
    [Header("Level Settings")]
    [SerializeField] private int startingChunksAmount = 12;
    [Tooltip("청크 프리팹 크기가 변경된 경우가 아니면 길이를 변경하지 마세요")]
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minMoveSpeed = 4f;
    [SerializeField] private float maxMoveSpeed = 16f;
    [SerializeField] private float minGravityZ = -20f;
    [SerializeField] private float maxGravityZ = -6f;

    List<GameObject> chunks = new List<GameObject>(); //리스트 생성

    void Start()
    {
        SpawnStartiongChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount; //새로운 이동 속도를 계산하여 newMoveSpeed 변수에 할당
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed); //newMoveSpeed를 minMoveSpeed와 maxMoveSpeed 사이로 제한하여 이동 속도가 너무 느리거나 빠르지 않도록 보장

        if (newMoveSpeed != moveSpeed) // newMoveSpeed가 현재 moveSpeed와 다를 때
        {
            moveSpeed = newMoveSpeed; //moveSpeed를 newMoveSpeed로 업데이트하여 이동 속도를 변경

            float newGravityZ = Physics.gravity.z - speedAmount; //새로운 중력 값을 계산하여 newGravityZ 변수에 할당
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
            cameraController.ChangeCameraFOV(speedAmount); //cameraController의 ChangeCameraFOV 메서드를 호출하여 카메라 FOV 변경
        }
    }

    private void SpawnStartiongChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++) // i가 startingChunksAmount보다 작을 시 i를 1씩 증가시키며 반복
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ(); //spawnPositionZ를 CalculateSpawnPositionZ() 메서드의 반환값으로 설정

        // chunkSpawnpos를 새로운 Vector3로 설정 (오브젝트의 현재 x, y 위치, spawnPositionZ)
        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        // 새로운 chunk 게임 오브젝트를 생성하여 newChunk 변수에 할당 (ChunkPrefab, chunkSpawnPos 위치, 회전 없음, chunkParent를 부모로 설정)
        GameObject newChunk = Instantiate(ChunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);

        chunks.Add(newChunk); //chunks 리스트에 newChunk를 추가하여 관리
    }

    private float CalculateSpawnPositionZ()
    {
        float spawnPositionZ;
        //chunks 리스트가 비어있을 때 spawnPositionZ를 현재 오브젝트의 z 위치로 설정, 그렇지 않으면 chunks 리스트의 마지막 요소의 z 위치에 chunkLength를 더한 값으로 설정
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ; //spawnPositionZ 반환
    }
    
    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++) // i가 chunks.Count보다 작을 시 i를 1씩 증가시키며 반복
        {
            if (chunks[i] != null) //chunks[i]가 null이 아닐 때
            {
                GameObject  chunk = chunks[i]; //chunks 리스트의 i번째 요소를 chunk라는 GameObject 변수에 할당
                chunk.transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime)); //chunk의 위치를 Vector3.back 방향으로 moveSpeed * Time.deltaTime만큼 이동
                
                //chunk의 z축 위치가 카메라의 z축 위치에서 chunkLength만큼 뒤에 있을 때
                if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
                {
                    chunks.Remove(chunk); //chunks 리스트에서 chunk를 제거
                    Destroy(chunk); //chunk 게임 오브젝트를 파괴
                    SpawnChunk(); //새로운 chunk를 생성하여 게임에 추가
                }
            }
        }
    }
}
 