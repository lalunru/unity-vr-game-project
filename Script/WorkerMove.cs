using UnityEngine;
using UnityEngine.AI;

public class WorkerMove : MonoBehaviour
{
    [Header("기본 설정")]
    public Transform target;       // 목적지
    public Transform player;       // 플레이어 (Main Camera로 설정하세요)
    public float detectRange = 20.0f; // 감지 거리

    [Header("심판관 연결")]
    public Rule3_Judge judge; //Rule3Judge

    [Header("기괴한 연출 연결")]
    public BoneBreaker[] horrorEffects; //BoneBreaker

    private NavMeshAgent agent;
    private Animator anim;
    private bool hasGreeted = false; // 인사를 시작했는지 체크
    private bool isFinished = false; // 상황이 끝났는지 체크

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (target != null) agent.SetDestination(target.position);

        // 시작할 때는 멀쩡하게
        ToggleHorror(false);
    }

    void Update()
    {
        if (player == null) return;

        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;

        Vector3 npcPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(npcPos, playerPos);

        // 1. 플레이어와 마주쳤을 때
        if (!isFinished && distance <= detectRange)
        {
            agent.isStopped = true;       // 멈춤
            anim.SetBool("isNear", true); // 인사 애니메이션 시작

            // 플레이어 쪽 쳐다보기
            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookPos);

            // 아직 인사를 안 했다면 -> 패턴 시작
            if (!hasGreeted)
            {
                hasGreeted = true; // "인사 했다"고 표시
                Debug.Log("인사 시작! 패턴 발동!");

                // 1. 뼈 꺾기 연출 켜기 (허리 꺾기)
                if (horrorEffects.Length > 0)
                {
                    if (horrorEffects[0] != null) horrorEffects[0].isBreaking = true;
                }

                // 2. 호출
                if (judge != null)
                {
                    judge.StartPattern();
                }
                else
                {
                    Debug.LogError("연결되지 않았습니다!");
                }
            }
        }
        // 2. 멀어졌거나, 이미 상황이 종료됐을 때
        else
        {
            agent.isStopped = false;       // 다시 걷기
            anim.SetBool("isNear", false); // 인사 끝

            // 뼈 펴기
            if (hasGreeted)
            {
                hasGreeted = false;
                ToggleHorror(false);
            }
        }
    }

    // BoneBreaker 끄고 켜는 함수
    void ToggleHorror(bool state)
    {
        foreach (var breaker in horrorEffects)
        {
            if (breaker != null) breaker.isBreaking = state;
        }
    }

    // 성공했다고 알려주면 호출되는 함수
    public void FinishEncounter()
    {
        Debug.Log("상황 종료! 갈 길 갑니다.");
        isFinished = true; // 이제 거리 무시하고 걸어감
        hasGreeted = false;
        ToggleHorror(false); // 몸 펴기
    }
}