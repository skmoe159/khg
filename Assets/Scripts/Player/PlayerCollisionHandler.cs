using System.Threading;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private Animator animator; // 애니메이터 컴포넌트를 인스펙터에서 설정할 수 있도록 SerializeField로 선언
    [SerializeField] private float hitCooldown = 1f; // 충돌 애니메이션의 지속 시간을 설정할 수 있도록 SerializeField로 선언
    [SerializeField] private float speedDecreaseAmount = -2f; // 충돌 시 이동 속도를 감소시키는 양을 설정할 수 있도록 SerializeField로 선언

    float cooldownTimer = 0f; // 충돌 애니메이션의 쿨다운 타이머를 초기화
    const string hit = "Hit"; // 충돌 애니메이션 트리거 이름을 저장하는 상수 문자열 변수 선언

    LevelGenerator levelGenerator; // LevelGenerator 스크립트의 참조를 저장할 변수 선언

    void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>(); // 게임 시작 시 LevelGenerator 스크립트를 찾아 참조를 저장
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime; // 매 프레임마다 쿨다운 타이머를 증가시켜 충돌 애니메이션의 지속 시간을 관리
    }

    void OnCollisionEnter(Collision collision) // 플레이어가 다른 오브젝트와 충돌했을 때 호출되는 메서드
    {
        if (collision.gameObject.CompareTag("Obstacle")) // 충돌한 오브젝트의 태그가 "Obstacle"인지 확인
        {
            if (cooldownTimer < hitCooldown) return; // 쿨다운 타이머가 hitCooldown보다 작을 때 메서드 종료하여 충돌 애니메이션이 연속으로 재생되는 것을 방지

            levelGenerator.ChangeChunkMoveSpeed(speedDecreaseAmount); // LevelGenerator의 ChangeChunkMoveSpeed 메서드를 호출하여 이동 속도를 감소시킴
            animator.SetTrigger(hit); // 애니메이터에서 "Hit" 트리거를 활성화하여 충돌 애니메이션을 재생
            cooldownTimer = 0f; // 쿨다운 타이머를 초기화하여 다음 충돌 애니메이션이 재생될 때까지 대기하도록 설정
        }
    }
}
