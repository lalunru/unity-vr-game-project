using UnityEngine;

public class Tutorial_MoveCheck : MonoBehaviour
{
    [Header("연결")]
    public Transform player;        // Final XR Origin
    public GameObject canvasObject; // TutMove 
    public GameObject nextTutorialCanvas;

    [Header("설정")]
    public float moveThreshold = 0.3f; // 0.3m 이동 감지

    private Vector3 startPos;
    private bool isFinished = false;

    void Start()
    {
        if (player != null) startPos = player.position;
    }

    void Update()
    {
        if (isFinished || player == null) return;

        // 플레이어가 일정 거리 이상 이동하면?
        if (Vector3.Distance(player.position, startPos) > moveThreshold)
        {
            // 1. 내 캔버스 끄기
            if (canvasObject != null) canvasObject.SetActive(false);

            // 2. 다음 튜토리얼 캔버스 켜기
            if (nextTutorialCanvas != null) nextTutorialCanvas.SetActive(true);

            isFinished = true;
            this.enabled = false;
        }
    }
}