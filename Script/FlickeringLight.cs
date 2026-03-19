using UnityEngine;
using System.Collections;

/// <summary>
/// 공포 연출용 조명 깜빡임 스크립트.
/// Light 컴포넌트와 머티리얼의 Emission을 동시에 토글해서
/// 전구가 실제로 깜빡이는 것처럼 보이게 한다.
/// </summary>
public class FlickeringLight : MonoBehaviour
{
    Light myLight;
    Renderer myRenderer;
    Color originalColor; // 머티리얼 원래 Emission 색상 저장용

    public float minWaitTime = 0.1f; // 깜빡임 최소 간격 (초)
    public float maxWaitTime = 0.5f; // 깜빡임 최대 간격 (초)

    void Start()
    {
        // Light 컴포넌트 탐색 (자식 오브젝트까지 확인)
        myLight = GetComponent<Light>();
        if (myLight == null) myLight = GetComponentInChildren<Light>();

        // Renderer 컴포넌트 탐색 (전구 메시용)
        myRenderer = GetComponent<Renderer>();
        if (myRenderer == null) myRenderer = GetComponentInChildren<Renderer>();

        // 원래 Emission 색상 저장 (꺼졌다 켜질 때 복원용)
        if (myRenderer != null)
            originalColor = myRenderer.material.GetColor("_EmissionColor");

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // 현재 상태의 반대로 토글
            bool isOn = !myLight.enabled;

            // 1. Light 컴포넌트 on/off
            if (myLight != null) myLight.enabled = isOn;

            // 2. 머티리얼 Emission on/off (조명과 동기화)
            if (myRenderer != null)
            {
                myRenderer.material.SetColor(
                    "_EmissionColor",
                    isOn ? originalColor : Color.black // 꺼질 때 Emission 제거
                );
            }

            // 랜덤 간격으로 다음 토글 대기
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }
}