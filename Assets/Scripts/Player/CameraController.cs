using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minFov = 20f; 
    [SerializeField] private float maxFov = 120f; 
    [SerializeField] private float zoomDuration = 1f; 
    [SerializeField] private float zoomSpeedAmount = 3f;
    [SerializeField] private ParticleSystem speedEffect;

    CinemachineVirtualCamera virtualCamera; //virtualCamera 변수 선언

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>(); //virtualCamera에 현재 게임 오브젝트의 CinemachineVirtualCamera 컴포넌트 할당
    }

    public void ChangeCameraFOV(float speedAmount)
    {
        StopAllCoroutines(); //모든 코루틴을 중지하여 이전 FOV 변경 작업을 취소
        StartCoroutine(ChangeFovRoutine(speedAmount)); //ChangeFovRoutine 코루틴을 시작하여 카메라 FOV 변경
        if (speedAmount < 0) return; //speedAmount가 0보다 작을 때 메서드 종료하여 속도 효과가 재생되지 않도록 함
            speedEffect.Play(); //speedEffect 파티클 시스템을 재생하여 속도 효과를 시각적으로 표현
    }

    IEnumerator ChangeFovRoutine(float speedAmount)
    {
        float startFov = virtualCamera.m_Lens.FieldOfView; //startFov를 현재 카메라의 FOV로 설정
        float targetFov = Mathf.Clamp(startFov + speedAmount * zoomSpeedAmount, minFov, maxFov); //targetFov를 startFov에 speedAmount를 더한 값으로 설정하되, minFov와 maxFov 사이로 제한
        float elapsedTime = 0f; //elapsedTime 변수 선언 및 초기화

        while (elapsedTime < zoomDuration) //elapsedTime이 zoomDuration보다 작을 때 반복
        {
            elapsedTime += Time.deltaTime; //elapsedTime에 Time.deltaTime을 더하여 경과 시간을 업데이트
            float t = elapsedTime / zoomDuration; //t를 elapsedTime을 zoomDuration으로 나눈 값으로 설정하여 보간 비율 계산
            
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startFov, targetFov, t); //카메라의 FOV를 startFov에서 targetFov로 선형 보간하여 설정
            yield return null; //다음 프레임까지 대기
        }
        virtualCamera.m_Lens.FieldOfView = targetFov; //카메라의 FOV를 targetFov로 최종 설정
    }
}
