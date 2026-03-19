using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 플레이어의 이동, 회전, 상호작용 기능을 일괄적으로 비활성화하는 스크립트
public class PlayerControllerDisabler : MonoBehaviour
{
    [Header("Movement Providers")]
    // XR Origin에 있는 이동 컴포넌트들
    public ContinuousMoveProviderBase moveProvider;
    public ContinuousTurnProviderBase turnProvider;

    [Header("Interactors & Controllers")]
    // 상호작용 컴포넌트 (Grab, Raycast 등을 막기 위함)
    public XRBaseController leftController;
    public XRBaseController rightController;

    public void DisableAllControls()
    {
        if (moveProvider != null) moveProvider.enabled = false;
        if (turnProvider != null) turnProvider.enabled = false;

        if (leftController != null) leftController.enabled = false;
        if (rightController != null) rightController.enabled = false;

    }
}