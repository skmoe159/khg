using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float xClamp = 3f;
    [SerializeField] private float zClamp = 1f;

    Vector2 movement;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PlayerMove();
    }

    public void Move(InputAction.CallbackContext context) // 플레이어의 이동 입력을 처리하는 메서드
    {
        movement = context.ReadValue<Vector2>(); // movement = context에서 Vector2 값을 읽어와 movement 변수에 저장
    }

    private void PlayerMove()
    {
        Vector3 currentPosition = rb.position; // currentPosition을 Rigidbody의 현재 위치로 설정
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y); // moveDirection을 movement의 x값과 y값을 사용하여 새로운 Vector3로 설정 (y축은 0으로 고정)
        // newPosition을 currentPosition에 moveDirection과 moveSpeed, Time.fixedDeltaTime을 곱한 값을 더하여 계산
        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp); // newPosition의 x값을 -xClamp와 xClamp 사이로 제한
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp); // newPosition의 z값을 -zClamp와 zClamp 사이로 제한

        rb.MovePosition(newPosition); // Rigidbody의 위치를 newPosition으로 이동
    }
}
