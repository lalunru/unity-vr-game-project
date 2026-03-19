using UnityEngine;
using System.Collections;

/// <summary>
/// Light 컴포넌트 없이 머티리얼 색상만으로 깜빡임을 구현하는 스크립트.
/// FlickeringLight와 달리 조명이 아닌 오브젝트 자체(전광판, 네온사인 등)에 사용.
/// 30% 확률로 긴 정지 구간을 넣어 불규칙한 고장난 느낌을 연출한다.
/// 
/// 주의: 이 스크립트를 사용할 오브젝트는 컴포넌트 이름을 
/// 'FlickeringLight'가 아닌 'FlickeringObject'로 구분할 것.
/// </summary>
public class FlickeringObject : MonoBehaviour
{
    [Header("설정")]
    public float minWaitTime = 0.05f; // 깜빡임 최소 간격
    public float maxWaitTime = 0.2f;  // 깜빡임 최대 간격

    [Range(0f, 1f)]
    public float dimFactor = 0.2f; // 어두워질 때 밝기 비율 (0=완전 꺼짐, 1=변화 없음)

    private Renderer myRenderer;
    private Color originalColor; // 원래 색상 저장용

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null)
        {
            // Emission 프로퍼티 여부에 따라 색상 저장 방식 분기
            if (myRenderer.material.HasProperty("_EmissionColor"))
                originalColor = myRenderer.material.GetColor("_EmissionColor");
            else
                originalColor = myRenderer.material.color;

            StartCoroutine(FlickerRoutine());
        }
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // 1. 밝게 (정상 상태)
            SetColor(originalColor);

            // 30% 확률로 0.5~2초 긴 정지 — 고장난 조명 느낌 연출
            if (Random.value > 0.7f)
                yield return new WaitForSeconds(Random.Range(0.5f, 2.0f));
            else
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            // 2. 어둡게 (깜빡임 상태)
            SetColor(originalColor * dimFactor);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }
    }

    // Emission 유무에 따라 색상 적용 방식 분기
    void SetColor(Color color)
    {
        if (myRenderer.material.HasProperty("_EmissionColor"))
            myRenderer.material.SetColor("_EmissionColor", color);
        else
            myRenderer.material.color = color;
    }
}