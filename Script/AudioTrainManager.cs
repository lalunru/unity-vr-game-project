using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioTrainManager : MonoBehaviour
{
    [Header("오디오 소스 연결")]
    public AudioSource trainArrivalSFX; // 열차 진입 소리
    public AudioSource trainStopSFX;    // 브레이크 소리
    public AudioSource urgentAlarmSFX;  // 안내방송
    public AudioSource screamSFX;       // 실패 시 비명/충돌음

    [Header("연출 효과")]
    public GameObject trainObject;      // 열차 오브젝트 
    public Light[] allLights;           // 맵의 모든 조명 

    void Start()
    {
        // 1. 게임 시작 5초 후 열차 소리 시작
        StartCoroutine(TrainArrivalSequence());
    }

    IEnumerator TrainArrivalSequence()
    {
        yield return new WaitForSeconds(3f);

        // 2. 진입 소리 재생 
        if (trainArrivalSFX != null) trainArrivalSFX.Play();
        Debug.Log("열차 소리: 벽 보세요!");

        // 소리 길이만큼 대기 
        yield return new WaitForSeconds(5f);

        // 3. 쿵! 하고 열차 등장
        if (trainObject != null) trainObject.SetActive(true);
        if (trainStopSFX != null) trainStopSFX.Play();

        Debug.Log("열차 정차 완료. 탑승 가능.");
    }

    // ★ 일지를 집었을 때 호출될 함수
    public void OnLogGrabbed()
    {
        // 중복 실행 방지
        if (urgentAlarmSFX.isPlaying) return;

        StartCoroutine(EmergencySequence());
    }

    IEnumerator EmergencySequence()
    {
        // 1. 긴박한 비상벨/안내방송 시작
        if (urgentAlarmSFX != null) urgentAlarmSFX.Play();
        Debug.Log("비상! 15초 안에 탈출하세요!");

        // 2. 15초 카운트다운
        yield return new WaitForSeconds(15f);

        // 3. 탈출 실패 (Game Over) 연출
        GameOverBlackout();
    }

    void GameOverBlackout()
    {
        Debug.Log("탈출 실패. 암전.");

        // 모든 소리 끄고 비명 지르기
        if (urgentAlarmSFX != null) urgentAlarmSFX.Stop();
        if (screamSFX != null) screamSFX.Play();

        // 맵의 모든 조명 꺼버리기
        foreach (Light light in allLights)
        {
            light.enabled = false;
        }

        // 2초 뒤 재시작
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}