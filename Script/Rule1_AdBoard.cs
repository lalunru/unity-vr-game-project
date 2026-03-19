using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Rule1_AdBoard : RuleEventBase
{
    [Header("Visual Effects")]
    public UnityEvent OnNoiseStart; // 노이즈 켜는 이벤트
    public UnityEvent OnNoiseStop;  // 노이즈 끄는 이벤트

    [Header("Sound (Optional)")]
    public AudioSource adBoardAudioSource;
    public AudioClip whisperSound;

    // 내부 변수
    private bool hasNoiseTriggered = false;

    private void Start()
    {
        // 시작할 때 혹시 켜져 있으면 끄기
        OnNoiseStop.Invoke();
    }

    public override void StartRule()
    {
        myRuleIndex = 0;
        isRuleCompleted = false;
        hasNoiseTriggered = false;

        // 소리 재생
        if (adBoardAudioSource != null && whisperSound != null)
        {
            adBoardAudioSource.clip = whisperSound;
            adBoardAudioSource.loop = true;
            adBoardAudioSource.Play();
            adBoardAudioSource.volume = 1f;
        }
    }

    // 전광판 앞 진입 시 (노이즈 켜기)
    public void TriggerNoiseEffect()
    {
        if (hasNoiseTriggered) return;

        Debug.Log(">>> 전광판 앞 진입! 노이즈 ON.");
        hasNoiseTriggered = true;

        if (OnNoiseStart != null) OnNoiseStart.Invoke();
    }

    // 탈출 시 (노이즈 끄기)
    public void OnPlayerExitArea()
    {
        if (isRuleCompleted) return;
        Debug.Log("레벨 1 성공! 노이즈 OFF.");

        if (adBoardAudioSource != null) adBoardAudioSource.Stop();

        if (OnNoiseStop != null) OnNoiseStop.Invoke();

        CompleteRule();
    }
}