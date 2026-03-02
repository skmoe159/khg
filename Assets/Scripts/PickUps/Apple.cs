using UnityEngine;

public class Apple : Pickup
{
    LevelGenerator levelGenerator; // LevelGenerator 스크립트의 참조를 저장할 변수 선언

    void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>(); // 게임 시작 시 LevelGenerator 스크립트를 찾아 참조를 저장
    }
    protected override void OnPickup()
    {
            // 사과를 획득했을 때의 동작을 구현하는 함수, 예를 들어 플레이어의 체력을 회복시키는 로직을 추가할 수 있습니다.
            levelGenerator.ChangeChunkMoveSpeed(2f); // LevelGenerator의 ChangeChunkMoveSpeed 메서드를 호출하여 이동 속도를 증가시킴
            Debug.Log("사과 획득! 이동 속도 증가!");
    }
}
