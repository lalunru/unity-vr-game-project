using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI; 

public class Rule3_Judge : MonoBehaviour
{
    [Header("플레이어 신체 연결")]
    public Transform playerHead; 

    [Header("시간 설정")]
    public float reactionTime = 3.0f; // 3초 동안은 봐줌
    public float holdTime = 2.0f;     // 그 후 2초간 더 버텨야 성공

    [Header("갑툭튀 설정")]
    public float jumpscareSpeed = 0.2f;
    public AudioClip screamSound;
    public BoneBreaker[] allBones; // 실패 시 비틀어버릴 뼈

    [Header("PC 테스트 모드")]
    public bool isDebugMode = true; // 켜두면 스페이스바로 성공 가능

    private float timer = 0f;
    private bool isJudging = false;
    private Vector3 lastHeadPos;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartPattern()
    {
        timer = 0f;
        isJudging = true;
        lastHeadPos = playerHead.position;
        Debug.Log("감시 시작! 3초 안에 바닥을 보고 멈추시오.");
    }

    void Update()
    {
        if (!isJudging) return;

        timer += Time.deltaTime;

        // --- 바닥 보기 & 멈추기 규칙 ---

        // 1. 고개 각도 체크 
        float lookDownAngle = playerHead.localEulerAngles.x;
        if (lookDownAngle > 180) lookDownAngle -= 360;

        // [PC 테스트용] 스페이스바 누르면 무조건 성공 처리
        bool pcCheat = isDebugMode && UnityEngine.InputSystem.Keyboard.current.spaceKey.isPressed;

        // 반응 시간(3초)이 지났는데도 바닥을 안 봤거나(각도 25도 미만),
        // 버티는 도중에 고개를 들어버리면 -> 실패!
        if (!pcCheat && timer > reactionTime && lookDownAngle < 25.0f)
        {
            TriggerFail("반응 늦음! (또는 도중에 고개 듦)");
            return;
        }

        // 2. 움직임 체크
        float moveSpeed = Vector3.Distance(playerHead.position, lastHeadPos) / Time.deltaTime;

        if (timer > 1.0f && moveSpeed > 0.5f)
        {
            TriggerFail("움직였다!");
            return;
        }

        lastHeadPos = playerHead.position;

        // 3. 성공 조건: (반응 시간 + 버티기 시간)을 모두 넘기면 성공
        if (timer > reactionTime + holdTime)
        {
            Success();
        }
    }

    void Success()
    {
        isJudging = false;
        Debug.Log("성공! 무사히 넘겼다.");

        WorkerMove worker = GetComponent<WorkerMove>();
        if (worker != null) worker.FinishEncounter();
    }

    void TriggerFail(string reason)
    {
        if (!isJudging) return;
        isJudging = false;
        Debug.Log("실패! " + reason);
        StartCoroutine(JumpscareRoutine());
    }

    System.Collections.IEnumerator JumpscareRoutine()
    {
        // 1. 이동 시스템 끄기
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.enabled = false; // 애니메이션 멈춤

        // 2. 온몸 비틀기
        foreach (var bone in allBones)
        {
            if (bone != null)
            {
                bone.isBreaking = true; // 꺾기 활성화
                bone.breakAngle = Random.Range(90f, 180f); // 랜덤하게 미친 듯이 꺾음
            }
        }

        // 3. 비명 지르기
        if (screamSound != null) audioSource.PlayOneShot(screamSound);

        // 4. 플레이어 얼굴로 돌진 (Lerp)
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos;

        while (t < 1.0f)
        {
            t += Time.deltaTime / jumpscareSpeed;

            // 플레이어 눈앞 20cm까지 돌진
            targetPos = playerHead.position + (playerHead.forward * 0.2f);
            targetPos.y = playerHead.position.y - 0.2f; // 눈높이 맞춤

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.LookAt(playerHead);

            yield return null;
        }

        // 5. 아주 잠깐 얼굴 보여주고 암전
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}