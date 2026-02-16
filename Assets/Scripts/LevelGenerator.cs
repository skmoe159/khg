using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ChunkPrefab;
    [SerializeField] private int startingChunksAmount = 12;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private float moveSpeed = 100f;

    List<GameObject> chunks = new List<GameObject>(); //리스트 생성

    void Start()
    {
        SpawnStartiongChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    private void SpawnStartiongChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++) // i가 0부터 시작해서 startingChunksAmount보다 작을 때까지 반복
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        GameObject newChunk = Instantiate(ChunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);

        chunks.Add(newChunk);
    }

    private float CalculateSpawnPositionZ()
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ;
    }
    
    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunks[i] != null)
            {
                GameObject  chunk = chunks[i];
                chunk.transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));
                
                if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
                {
                    chunks.Remove(chunk);
                    Destroy(chunk);
                    SpawnChunk();
                }
            }
        }
    }
}
 