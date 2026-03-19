using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// VR 컨트롤러 버튼으로 매뉴얼(업무 수칙) UI를 on/off 토글하는 스크립트.
/// 왼손 컨트롤러의 Primary Button(X버튼)에 바인딩해서 사용.
/// Inspector에서 toggleInput의 Binding을
/// 'XR Controller (Left Hand) -> Primary Button'으로 설정할 것.
/// </summary>
public class ManualToggle : MonoBehaviour
{
    [Header("대상")]
    [Tooltip("열고 닫을 매뉴얼 오브젝트 (Canvas 또는 3D 오브젝트)")]
    public GameObject manualObject;

    [Header("입력 설정")]
    [Tooltip("Inspector에서 Binding을 'XR Controller (Left Hand) -> Primary Button'으로 설정하세요.")]
    public InputActionProperty toggleInput;

    private void OnEnable()
    {
        if (toggleInput.action != null)
            toggleInput.action.Enable();
    }

    // 오브젝트가 비활성화되거나 파괴될 때 반드시 Disable — 메모리 누수 방지
    private void OnDisable()
    {
        if (toggleInput.action != null)
            toggleInput.action.Disable();
    }

    void Update()
    {
        // 버튼을 누른 첫 프레임에만 반응
        if (toggleInput.action != null && toggleInput.action.WasPressedThisFrame())
            ToggleManual();
    }

    // 매뉴얼 오브젝트 활성 상태 반전
    void ToggleManual()
    {
        if (manualObject != null)
        {
            manualObject.SetActive(!manualObject.activeSelf);
            Debug.Log($"매뉴얼 현재 상태: {manualObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning("ManualToggle: manualObject가 할당되지 않았습니다!");
        }
    }
}