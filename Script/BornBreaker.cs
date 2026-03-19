using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR 공포 연출용 뼈 꺾기 효과 스크립트.
/// 특정 본(Bone)의 로컬 회전값을 강제로 덮어써서
/// 애니메이션 위에 추가적인 변형을 가한다.
/// </summary>
public class BoneBreaker : MonoBehaviour
{
    [Header("대상")]
    public Transform targetBone; // 변형할 본 (Spine 또는 Hand)

    [Tooltip("체크하면 활성화됩니다. (성능 고려로 필요할 때만 켜세요)")]
    public bool isBreaking = false;

    [Header("뼈 설정")]
    public Vector3 breakAxis = new Vector3(1, 0, 0); // 회전축 — (1,0,0)은 X축(꺾이기)
    public float breakAngle = 90.0f;                  // 꺾이는 각도

    /// <summary>
    /// LateUpdate: 애니메이션이 적용된 직후에 호출되므로
    /// 애니메이터의 본 회전값을 덮어쓰기에 적합한 타이밍.
    /// </summary>
    void LateUpdate()
    {
        if (isBreaking && targetBone != null)
        {
            // 현재 애니메이션 회전값에 추가 회전을 곱해서 변형 적용
            targetBone.localRotation *= Quaternion.Euler(breakAxis * breakAngle);
        }
    }
}