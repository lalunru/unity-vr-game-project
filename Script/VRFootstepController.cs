using UnityEngine;

public class VRFootstepController : MonoBehaviour
{
    [Header("설정")]
    public float stepDistance = 8f; // 발소리 간격
    public float minVolume = 0.5f;    // 소리 크기 최소
    public float maxVolume = 1f;    // 소리 크기 최대

    [Header("연결")]
    public AudioSource footstepSource; // 발소리 낼 스피커
    public AudioClip[] stepSounds;     // 발소리 파일

    private Vector3 lastPosition;
    private float distanceMoved = 0f;

    void Start()
    {
        // 시작 위치 기억
        lastPosition = transform.position;

        // 오디오 소스 없으면 자동으로 추가
        if (footstepSource == null)
            footstepSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // 1. 이번 프레임에 얼마나 움직였는지 계산 (Y축은 무시해서 수평 이동량만 계산)
        Vector3 currentPos = transform.position;
        float moveAmount = Vector3.Distance(new Vector3(lastPosition.x, 0, lastPosition.z),
                                            new Vector3(currentPos.x, 0, currentPos.z));

        // 2. 이동량이 너무 크면(텔레포트 등) 발소리 건너뜀
        if (moveAmount > 2.0f)
        {
            distanceMoved = 0f;
        }
        else
        {
            distanceMoved += moveAmount;
        }

        // 3. 누적 이동 거리가 설정한 보폭을 넘으면 소리 재생
        if (distanceMoved >= stepDistance)
        {
            PlayFootstep();
            distanceMoved = 0f; // 거리 초기화
        }

        lastPosition = currentPos;
    }

    void PlayFootstep()
    {
        if (stepSounds.Length == 0) return;

        // 랜덤한 소리 하나 고르기
        int randIndex = Random.Range(0, stepSounds.Length);

        // 피치와 볼륨을 살짝 랜덤하게 줘서 자연스럽게 만듦
        footstepSource.pitch = Random.Range(0.9f, 1.1f);
        footstepSource.volume = Random.Range(minVolume, maxVolume);

        // 재생
        footstepSource.PlayOneShot(stepSounds[randIndex]);
    }
}