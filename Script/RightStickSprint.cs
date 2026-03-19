using UnityEngine;
using UnityEngine.InputSystem; // 입력 시스템 사용
using UnityEngine.XR.Interaction.Toolkit; // 이동 시스템 사용

public class RightStickSprint : MonoBehaviour
{
    [Header("Settings")]
    public float runSpeed = 10f; // 달릴 때 속도
    private float walkSpeed;     // 원래 걷는 속도 (자동 저장)

    [Header("References")]
    public ActionBasedContinuousMoveProvider moveProvider; // 이동 담당 컴포넌트

    // 오른쪽 스틱 입력을 받아올 변수
    public InputActionProperty sprintInput;

    void Start()
    {
        // 시작할 때 원래 걷는 속도를 기억해둠
        if (moveProvider != null)
        {
            walkSpeed = moveProvider.moveSpeed;
        }
    }

    void Update()
    {
        if (moveProvider == null) return;

        // 오른쪽 스틱의 움직임 값을 읽어옴
        Vector2 input = sprintInput.action.ReadValue<Vector2>();

        // 스틱을 앞으로(Y축) 0.5 이상 밀었을 때 -> 달리기
        if (input.y > 0.5f)
        {
            moveProvider.moveSpeed = runSpeed;
        }
        else
        {
            // 스틱을 놓으면 -> 다시 걷기 속도로 복귀
            moveProvider.moveSpeed = walkSpeed;
        }
    }
}