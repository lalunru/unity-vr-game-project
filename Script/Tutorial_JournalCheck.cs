using UnityEngine;

public class Tutorial_JournalCheck : MonoBehaviour
{
    [Header("연결")]
    public GameObject tutorialCanvas;
    public GameObject realJournalUI;  // 실제 게임의 업무일지 UI

    void Update()
    {
        if (realJournalUI == null) return;

        // 실제 업무일지가 켜졌다면? (플레이어가 X버튼을 눌러서 열었다면)
        if (realJournalUI.activeInHierarchy)
        {
            Debug.Log("일지 튜토리얼 완료!");

            // 안내 문구 끄기
            if (tutorialCanvas != null) tutorialCanvas.SetActive(false);

            this.enabled = false; // 스크립트 종료
        }
    }
}