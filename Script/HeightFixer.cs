using UnityEngine;

public class HeightFixer : MonoBehaviour
{
    // 원하는 키 높이 (미터 단위)
    public float targetHeight = 1.7f;
    public Transform cameraOffset;

    void Update()
    {
        if (cameraOffset != null)
        {
            // 현재 카메라 오프셋의 X, Z는 유지하고 Y(높이)만 강제로 고정
            cameraOffset.localPosition = new Vector3(cameraOffset.localPosition.x, targetHeight, cameraOffset.localPosition.z);
        }
    }
}