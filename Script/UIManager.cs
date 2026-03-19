using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Coroutine hideMessageCoroutine;

    public TextMeshProUGUI systemMessageText; // 시스템 메시지
    public GameObject journalUI;             // 수첩 UI 오브젝트
    public TextMeshProUGUI timerText;         // 타이머 UI
    public GameObject distortionEffect;      // 화면 왜곡 효과

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 시스템 메시지를 N초간 보여주는 함수
    public void ShowSystemMessage(string message, float duration)
    {
        // 1. 이전 코루틴 중지
        if (hideMessageCoroutine != null)
        {
            StopCoroutine(hideMessageCoroutine);
        }

        if (systemMessageText != null)
        {
            // 2. 메시지 표시
            systemMessageText.gameObject.SetActive(true);
            systemMessageText.text = message;

            // 3. 코루틴 시작
            hideMessageCoroutine = StartCoroutine(HideMessageAfterDelay(duration));
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (systemMessageText != null)
        {
            systemMessageText.text = ""; // 텍스트 내용 비우기
            systemMessageText.gameObject.SetActive(false); // UI 오브젝트 비활성화
        }
        hideMessageCoroutine = null; // 코루틴이 끝났음을 표시
    }

    // 타이머 UI를 업데이트하는 함수
    public void UpdateTimer(float time)
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.text = time.ToString("F1");
        }
    }

    // 수첩 UI 켜기/끄기
    public void ToggleJournal(bool show)
    {
        if (journalUI != null) journalUI.SetActive(show);
    }
}