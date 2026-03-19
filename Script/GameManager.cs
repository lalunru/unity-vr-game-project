using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 게임 전체 흐름을 관리하는 중앙 컨트롤러 (싱글톤).
/// 규칙 순차 진행, 씬 전환, 페이드 인/아웃, 성공/실패 처리를 담당한다.
/// 모든 RuleEventBase 스크립트는 이 클래스를 통해 완료/실패를 보고한다.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameFinished = false;

    [Header("레벨 정보")]
    public string thisSceneName;  // 현재 씬 이름
    public string nextSceneName;  // 클리어 후 이동할 씬 이름

    [Header("UI")]
    public CanvasGroup fadePanel;    // 페이드 인/아웃용 검은 패널
    public float fadeDuration = 1.5f; // 페이드 지속 시간 (초)

    [Header("규칙 관리")]
    public int currentRuleID = 0;
    public RuleEventBase[] currentLevelRules; // 이 레벨에서 순서대로 실행할 규칙 배열

    [Header("엔딩 오브젝트")]
    public GameObject ending1_Objects;
    public GameObject ending2_Objects;

    [Header("플레이어")]
    public PlayerControllerDisabler playerControllerDisabler;

    [Header("오디오")]
    public AudioClip mainBGMClip;
    public AudioClip failSFXClip;
    public AudioClip successSFXClip;

    void Awake()
    {
        // 싱글톤 설정 — 씬 내 하나의 인스턴스만 유지
        if (Instance == null) Instance = this;
        else Instance = this;
    }

    void Start()
    {
        thisSceneName = SceneManager.GetActiveScene().name;
        isGameFinished = false;
        currentRuleID = 0;

        // 씬 시작 시 검은 화면에서 서서히 밝아지는 Fade In
        StartCoroutine(FadeInRoutine());

        // BGM 재생
        if (SoundManager.Instance != null && mainBGMClip != null)
            SoundManager.Instance.PlayBGM(mainBGMClip);

        // 첫 번째 규칙 자동 시작
        if (currentLevelRules != null && currentLevelRules.Length > 0)
        {
            currentLevelRules[0].StartRule();
            currentRuleID = 1;
        }

        Debug.Log(thisSceneName + " 레벨이 시작됩니다.");
    }

    // 씬 진입 시 Fade In (검정 → 화면)
    IEnumerator FadeInRoutine()
    {
        if (fadePanel == null) yield break;

        fadePanel.alpha = 1f;
        fadePanel.blocksRaycasts = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadePanel.alpha = 1f - t;
            yield return null;
        }

        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = false;
    }

    /// <summary>
    /// 규칙 완료 시 RuleEventBase에서 호출.
    /// 다음 규칙을 순서대로 시작하거나, 마지막 규칙이면 레벨 클리어 처리.
    /// </summary>
    public void NotifyRuleCompleted(int completedRuleIndex)
    {
        if (isGameFinished) return;

        Debug.Log("규칙 #" + (completedRuleIndex + 1) + " 완료 처리.");

        if (SoundManager.Instance != null && successSFXClip != null)
            SoundManager.Instance.PlaySFX(successSFXClip);

        int nextRuleIndex = completedRuleIndex + 1;

        if (nextRuleIndex < currentLevelRules.Length)
        {
            // 다음 규칙 시작
            currentRuleID = nextRuleIndex + 1;
            currentLevelRules[nextRuleIndex].StartRule();
            Debug.Log("다음 규칙 #" + currentRuleID + "을 시작합니다.");
        }
        else
        {
            // 모든 규칙 완료 → 레벨 클리어
            CompleteLevel();
        }
    }

    // 레벨 클리어 처리 — BGM 정지 후 다음 씬으로 이동
    public void CompleteLevel()
    {
        if (isGameFinished) return;

        Debug.Log(thisSceneName + " 레벨 클리어.");

        if (SoundManager.Instance != null)
            SoundManager.Instance.StopBGM();

        if (!string.IsNullOrEmpty(nextSceneName))
            StartCoroutine(FadeOutAndLoadScene(nextSceneName));
        else
            StartFinalProcedure();
    }

    // Fade Out 후 씬 이동 (화면 → 검정 → 씬 로드)
    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            fadePanel.blocksRaycasts = true;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / fadeDuration;
                fadePanel.alpha = t;
                yield return null;
            }
            fadePanel.alpha = 1f;
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    // 마지막 레벨 클리어 시 최종 엔딩 절차 시작
    public void StartFinalProcedure()
    {
        Debug.Log("모든 레벨 클리어. 최종 절차(Rule 5)를 시작합니다.");
    }

    /// <summary>
    /// 규칙 실패 시 호출.
    /// 실패 효과음 재생 후 Fade Out → 현재 씬 재시작.
    /// </summary>
    public void FailRule()
    {
        if (isGameFinished) return;

        Debug.Log("규칙 실패. 재시작.");

        if (SoundManager.Instance != null && failSFXClip != null)
            SoundManager.Instance.PlaySFX(failSFXClip);

        StartCoroutine(ResetSceneWithFade(2f));
    }

    // 실패 시 Fade Out 후 현재 씬 재시작
    IEnumerator ResetSceneWithFade(float delay)
    {
        // 효과음 재생 시간 확보 후 페이드 시작
        yield return new WaitForSeconds(delay - 1.0f);

        if (fadePanel != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / 1.0f;
                fadePanel.alpha = t;
                yield return null;
            }
        }

        SceneManager.LoadScene(thisSceneName);
    }

    // 엔딩 시작 (endingID로 분기 — 추후 구현)
    public void StartEnding(int endingID)
    {
        // 엔딩 분기 처리 예정
    }

    // 플레이어 모든 컨트롤 비활성화 (엔딩 연출 중 사용)
    private void DisablePlayerControls()
    {
        if (playerControllerDisabler != null)
            playerControllerDisabler.DisableAllControls();
    }
}