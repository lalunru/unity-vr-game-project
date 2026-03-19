using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rule2_train2 : RuleEventBase
{
    [Header("Test Settings")]
    public bool playOnStart = false;       // 게임 시작 시 자동 테스트용

    [Header("Settings")]
    public float arrivalDuration = 5f;     // 열차 진입 소리 들리는 시간
    public float escapeTimeLimit = 15f;    // 탈출 제한 시간
    public float lookAngleThreshold = 60f; // 뒤돌아봄 감지 각도

    [Header("Train Departure Settings")]   // 열차 출발 연출 설정
    public float departureDelay = 3.0f;    // 탈출 후 출발 대기 시간
    public float departureSpeed = 15.0f;   // 열차 이동 속도
    public float departureDuration = 6.0f; // 이동하는 시간

    [Header("References")]
    public Transform playerHead;           // Main Camera
    public Transform trainSpawnPoint;      // 열차 위치
    public GameObject trainObject;         // 열차 모델

    [Header("Audio Sources")]
    public AudioSource trainArrivalSFX;    // 진입/출발 소리 
    public AudioSource trainStopSFX;       // 정차 소리
    public AudioSource urgentAlarmSFX;     // 비상벨
    public AudioSource screamSFX;          // 실패 비명

    [Header("Visual Effects")]
    public Light[] allLights;              // 실패 시 끌 조명들

    // 내부 상태 변수
    private bool hasGrabbedLog = false;
    private bool isEscaping = false;

    private void Start()
    {
        if (playOnStart && playerHead != null && trainSpawnPoint != null)
        {
            StartRule();
        }
    }

    public override void StartRule()
    {
        myRuleIndex = 1;
        isRuleCompleted = false;
        hasGrabbedLog = false;
        isEscaping = false;

        if (trainObject != null) trainObject.SetActive(false);

        StartCoroutine(TrainArrivalSequence());
    }

    // 1. 열차 진입 시퀀스 (시선 감지 포함)
    IEnumerator TrainArrivalSequence()
    {
        yield return new WaitForSeconds(3f); // 3초 대기

        if (trainArrivalSFX != null) trainArrivalSFX.Play();
        Debug.Log("열차 진입 중! 절대 뒤돌아보지 마시오!");

        float timer = 0f;
        while (timer < arrivalDuration)
        {
            timer += Time.deltaTime;

            // 뒤돌아봄 감지
            if (CheckIfPlayerLooksAtTrain())
            {
                Debug.Log("뒤를 돌아봤다! 게임 오버!");
                GameOverBlackout();
                yield break; // 코루틴 강제 종료
            }
            yield return null;
        }

        // 도착 완료
        if (trainArrivalSFX != null) trainArrivalSFX.Stop();

        // 열차 등장
        if (trainObject != null) trainObject.SetActive(true);
        if (trainStopSFX != null) trainStopSFX.Play();

        Debug.Log("열차 정차 완료. 탑승하여 서류를 찾으세요.");
    }

    private bool CheckIfPlayerLooksAtTrain()
    {
        if (playerHead == null || trainSpawnPoint == null) return false;

        Vector3 directionToTrain = (trainSpawnPoint.position - playerHead.position).normalized;
        float angle = Vector3.Angle(playerHead.forward, directionToTrain);

        return angle < lookAngleThreshold;
    }

    // 2. 서류 집었을 때
    public void OnLogGrabbed()
    {
        if (hasGrabbedLog) return;
        hasGrabbedLog = true;

        StartCoroutine(EmergencySequence());
    }

    IEnumerator EmergencySequence()
    {
        if (urgentAlarmSFX != null) urgentAlarmSFX.Play();
        isEscaping = true;
        Debug.Log("서류 확보! 15초 내에 열차 밖으로 나가세요!");

        yield return new WaitForSeconds(escapeTimeLimit);

        // 시간 내에 못 나갔으면 사망
        if (!isRuleCompleted && isEscaping)
        {
            Debug.Log("시간 초과! 탈출 실패.");
            GameOverBlackout();
        }
    }

    public void OnPlayerEscapedTrain()
    {
        if (hasGrabbedLog && isEscaping)
        {
            Debug.Log("탈출 성공! 규칙 2 완수.");
            isEscaping = false;

            if (urgentAlarmSFX != null) urgentAlarmSFX.Stop();

            StartCoroutine(TrainDepartureRoutine());

            CompleteRule();
        }
    }

    IEnumerator TrainDepartureRoutine()
    {
        Debug.Log($"{departureDelay}초 후 열차가 출발합니다...");
        yield return new WaitForSeconds(departureDelay);

        Debug.Log("열차 출발!");

        float startVol = 0.6f;
        if (trainArrivalSFX != null)
        {
            trainArrivalSFX.volume = startVol;
            trainArrivalSFX.Play();
        }
        yield return new WaitForSeconds(departureDelay);

        Debug.Log("이동 루프 진입! 열차 오브젝트 상태: " + (trainObject != null));

        float elapsed = 0f;
        while (elapsed < departureDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / departureDuration;

            // 열차 이동
            if (trainObject != null)
            {
                trainObject.transform.Translate(Vector3.forward * departureSpeed * Time.deltaTime, Space.Self);
            }

            // 소리 점점 작아지게
            if (trainArrivalSFX != null)
                trainArrivalSFX.volume = Mathf.Lerp(startVol, 0f, t);

            yield return null;
        }

        // 마무리
        if (trainObject != null) trainObject.SetActive(false);
        if (trainArrivalSFX != null) trainArrivalSFX.Stop();
    }

    void GameOverBlackout()
    {
        if (trainArrivalSFX != null) trainArrivalSFX.Stop();
        if (urgentAlarmSFX != null) urgentAlarmSFX.Stop();
        if (screamSFX != null) screamSFX.Play();

        foreach (Light light in allLights)
        {
            if (light != null) light.enabled = false;
        }

        FailRule();
    }
}