using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/// <summary>
/// 손전등 모드 전환 컨트롤러.
/// 일반 조명(흰색)과 UV 조명(자외선) 두 가지 모드를 토글하며,
/// 각 모드에 따라 보이는 힌트 오브젝트가 달라진다.
/// UV 모드에서만 숨겨진 코드('6179')가 보인다.
/// </summary>
public class FlashlightController : MonoBehaviour
{
    [Header("조명")]
    public Light spotlight;         // 일반 흰색 손전등
    public GameObject uvLightObj;   // UV 자외선 손전등

    [Header("힌트 오브젝트")]
    public GameObject normalHintObject; // 일반 모드에서 보이는 힌트 ('8638')
    public GameObject uvHintObject;     // UV 모드에서만 보이는 숨겨진 힌트 ('6179')

    [Header("입력")]
    public InputActionProperty toggleModeInput; // 모드 전환 버튼 (VR 컨트롤러)

    [Header("레이어 설정")]
    public LayerMask normalMask; // 일반 조명 컬링 마스크
    public LayerMask uvMask;     // UV 조명 컬링 마스크

    private bool isUVMode = false;
    private Light uvLightComponent;

    // 현재 실행 중인 텍스트 표시 코루틴 참조 (버튼 연타 방지용)
    private Coroutine revealCoroutine;

    void Start()
    {
        if (uvLightObj != null) uvLightComponent = uvLightObj.GetComponent<Light>();

        // 각 조명의 컬링 마스크 초기 설정
        if (spotlight != null) spotlight.cullingMask = normalMask;
        if (uvLightComponent != null) uvLightComponent.cullingMask = uvMask;

        // 시작 시 딜레이 없이 일반 모드 적용
        ApplyLightMode(false);
    }

    void Update()
    {
        // 모드 전환 버튼 입력 감지
        if (toggleModeInput.action != null && toggleModeInput.action.WasPressedThisFrame())
        {
            isUVMode = !isUVMode;
            // 버튼 입력 시 2초 딜레이 후 힌트 표시
            ApplyLightMode(true);
        }
    }

    /// <summary>
    /// 조명 모드 적용.
    /// useDelay가 true면 힌트가 2초 후에 나타난다 (연출 효과).
    /// </summary>
    void ApplyLightMode(bool useDelay)
    {
        // 1. 모든 힌트 오브젝트 우선 숨기기
        if (normalHintObject != null) normalHintObject.SetActive(false);
        if (uvHintObject != null) uvHintObject.SetActive(false);

        // 2. 현재 모드에 맞게 조명 전환
        if (isUVMode)
        {
            if (spotlight != null) spotlight.enabled = false;
            if (uvLightObj != null) uvLightObj.SetActive(true);
        }
        else
        {
            if (spotlight != null) spotlight.enabled = true;
            if (uvLightObj != null) uvLightObj.SetActive(false);
        }

        // 3. 이미 실행 중인 코루틴이 있으면 중단 (버튼 연타 시 중복 방지)
        if (revealCoroutine != null) StopCoroutine(revealCoroutine);

        if (useDelay)
            revealCoroutine = StartCoroutine(RevealTextRoutine());
        else
            ShowCurrentText();
    }

    // 2초 딜레이 후 힌트 표시 (조명 전환 연출)
    IEnumerator RevealTextRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        ShowCurrentText();
    }

    // 현재 모드에 맞는 힌트 오브젝트 활성화
    void ShowCurrentText()
    {
        if (isUVMode)
        {
            if (uvHintObject != null) uvHintObject.SetActive(true);
        }
        else
        {
            if (normalHintObject != null) normalHintObject.SetActive(true);
        }
    }
}