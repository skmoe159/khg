using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f; // 아이템의 회전 속도를 조절하는 변수
    
    const string playerString = "Player"; // 플레이어 태그를 저장하는 문자열 변수 선언
    
    void Update()
    {
        Rotate(); // 매 프레임마다 아이템을 회전시키는 함수 호출
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString)) // 충돌한 오브젝트의 태그가 "Player"인지 확인
        {
            OnPickup(); // 아이템이 획득되었을 때 추상 메서드 OnPickup() 호출
            Destroy(gameObject); // 아이템 게임 오브젝트를 파괴하여 제거
            // 여기에서 아이템 획득 처리나 플레이어의 상태 변경 등의 로직을 추가할 수 있습니다.
        }
    }
    void Rotate()
    {
        float value = rotationSpeed * Time.deltaTime;
        // 아이템을 회전시키는 함수, 회전 속도는 rotationSpeed 변수에 의해 조절
        transform.Rotate(Vector3.up, value); // Y축을 기준으로 회전
    }

    protected abstract void OnPickup(); // 아이템이 획득될 때 실행되는 추상 메서드, 각 아이템마다 다른 동작을 구현하기 위해 선언
}
