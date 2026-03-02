using UnityEngine;

public class Coin : Pickup
{
    protected override void OnPickup()
    {
        // 사과를 획득했을 때의 동작을 구현하는 함수, 예를 들어 플레이어의 체력을 회복시키는 로직을 추가할 수 있습니다.
        Debug.Log("코인 획득! 점수 획득!"); // 사과가 획득되었음을 로그로 출력
    }

}
