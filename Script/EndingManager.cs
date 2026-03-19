using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 엔딩 연출 관리 스크립트.
/// 화면에 메시지를 타이핑 효과로 순차 출력하고,
/// 발소리 → 페이드 아웃 → 타이틀 씬 이동까지의 흐름을 제어한다.
/// </summary>

public class EndingManager : MonoBehaviour
{
    [Header("1. 화면 설정")]
    public TMP_Text displayScreen;

    [Header("2. 출력할 내용")]
    [TextArea]
    public string message1 = "코드 6179 확인\n\n사용자 지정이 '관측자'로 갱신되었습니다.";

    [TextArea(5, 10)]
    public string message2 =
        "1. 당신은 이곳을 떠날 수 없습니다.\n" +
        "2. 당신은 다음에 배정될 '임시직'의 활동을\n" + "관측해야 합니다.\n" +
        "3. 당신은 '임시직'에게 직접 개입할 수 없습니다.\n" +
        "4. 만약 '임시직'이 당신의 존재를 인지할 경우,\n" +
        "    당신은 '역무원'의 역할을 수행해야 합니다.\n\n" +
        "<color=red>본 직무는 즉시 효력이 발생합니다.</color>";

    [Header("3. 소리 & 암전")]
    public AudioSource footstepAudio;
    public CanvasGroup fadePanel;
    public AudioSource typingAudioSource; // 타자 소리 낼 스피커
    public AudioClip typingLongClip;      // 긴 타자 소리 파일 (Loop용)

    [Header("4. 플레이어 제어")]
    public GameObject locomotionSystemObject;

    [Header("5. 씬 이동 설정")]
    public string titleSceneName = "TitleScene";

    void Start()
    {
        if (typingAudioSource == null)
            typingAudioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(EndingRoutine());
    }

    // 소리 켜기 (반복 재생)
    void StartTypingSound()
    {
        if (typingAudioSource != null && typingLongClip != null)
        {
            typingAudioSource.clip = typingLongClip;
            typingAudioSource.loop = true; // 무한 반복
            typingAudioSource.Play();
        }
    }

    // 소리 끄기
    void StopTypingSound()
    {
        if (typingAudioSource != null)
        {
            typingAudioSource.Stop();
        }
    }

    IEnumerator EndingRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        if (locomotionSystemObject != null)
        {
            locomotionSystemObject.SetActive(false);
            Debug.Log("플레이어 이동 시스템 차단됨");
        }

        // === 1단계: 코드 확인 메시지 ===
        displayScreen.text = "";
        displayScreen.color = Color.green;
        displayScreen.alpha = 1.0f;

        StartTypingSound(); 

        foreach (char letter in message1.ToCharArray())
        {
            displayScreen.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        StopTypingSound(); 

        yield return new WaitForSeconds(3.0f);

        // === 2단계: 메시지 사라짐 ===
        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 2.0f;
            displayScreen.alpha = 1.0f - t;
            yield return null;
        }

        // === 3단계: 새로운 수칙 출력 ===
        displayScreen.text = message2;
        displayScreen.maxVisibleCharacters = 0;
        displayScreen.color = Color.white;
        displayScreen.alpha = 1.0f;

        displayScreen.ForceMeshUpdate();
        int totalChars = displayScreen.textInfo.characterCount;

        StartTypingSound();

        for (int i = 0; i <= totalChars; i++)
        {
            displayScreen.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.05f);
        }

        StopTypingSound();

        // === 4단계: 발소리 ===
        yield return new WaitForSeconds(1.0f);

        if (footstepAudio != null) footstepAudio.Play();

        yield return new WaitForSeconds(4.0f);

        // === 5단계: 암전 ===
        t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime / 4.0f;
            if (fadePanel != null) fadePanel.alpha = t;
            yield return null;
        }

        // === 6단계: 타이틀 이동 ===
        Debug.Log("암전 완료. 2초 후 타이틀로 이동합니다.");
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(titleSceneName);
    }
}