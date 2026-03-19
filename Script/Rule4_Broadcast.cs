using UnityEngine;
using UnityEngine.Events;

public class Rule4_Broadcast : RuleEventBase
{
    [Header("Test Settings")]
    public bool playOnStart = false;

    [Header("Settings")]
    public float timeLimit = 30f;
    public float distortionStartTime = 10f;

    [Header("References")]
    public AudioSource speakerAudioSource;
    public AudioSource extraAudioSource;

    [Header("Audio Clips")]
    public AudioClip normalClip;
    public AudioClip distortedClip1;
    public AudioClip distortedClip2;

    [Header("Visual Effects")]
    public UnityEvent OnDistortionStart;
    public UnityEvent OnRuleFail;

    // 내부 변수
    private float currentTimer = 0f;
    private bool isDistorted = false;
    private bool isRuleActive = false;

    private void Start()
    {
        if (playOnStart)
        {
            StartRule();
        }
    }

    public override void StartRule()
    {
        if (isRuleActive) return;

        // myRuleIndex = 3; // (인덱스는 프로젝트 설정에 맞게)
        isRuleCompleted = false;
        isRuleActive = true;

        currentTimer = 0f;
        isDistorted = false;

        Debug.Log(">>> 규칙 4 진입! 안내 방송 시작.");

        if (speakerAudioSource != null && normalClip != null)
        {
            speakerAudioSource.clip = normalClip;
            speakerAudioSource.loop = true;
            speakerAudioSource.Play();
            speakerAudioSource.volume = 0.5f;
        }
    }

    void Update()
    {
        if (isRuleCompleted || !isRuleActive) return;

        currentTimer += Time.deltaTime;

        // 1. 왜곡 시작
        if (!isDistorted && currentTimer >= distortionStartTime)
        {
            isDistorted = true;
            Debug.Log("방송 왜곡 시작!");

            if (speakerAudioSource != null)
            {
                speakerAudioSource.Stop();
                if (distortedClip1 != null)
                {
                    speakerAudioSource.clip = distortedClip1;
                    speakerAudioSource.Play();
                    speakerAudioSource.volume = 0.5f;
                }
            }

            if (extraAudioSource != null && distortedClip2 != null)
            {
                extraAudioSource.clip = distortedClip2;
                extraAudioSource.loop = true;
                extraAudioSource.Play();
            }

            OnDistortionStart.Invoke();
        }

        // 2. 시간 초과 (실패)
        if (currentTimer >= timeLimit)
        {
            Debug.Log("시간 초과! 방송 차단 실패.");
            StopAllSounds();

            OnRuleFail.Invoke();

            FailRule();

            isRuleActive = false;
        }
    }

    public void StopBroadcast()
    {
        if (isRuleCompleted) return;

        Debug.Log("성공! 방송 차단 완료.");
        StopAllSounds();

        CompleteRule();
    }

    protected new void CompleteRule()
    {
        base.CompleteRule();
        isRuleActive = false;
    }

    private void StopAllSounds()
    {
        if (speakerAudioSource != null) speakerAudioSource.Stop();
        if (extraAudioSource != null) extraAudioSource.Stop();
    }
}