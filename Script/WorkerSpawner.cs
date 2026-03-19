using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [Header("연결")]
    public GameObject workerNPC; // 소환할 역무원 오브젝트 (처음엔 꺼둘 것)

    [Header("옵션")]
    public bool spawnOnce = true;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("무언가 닿았다! 이름: " + other.name + " / 태그: " + other.tag);
        // 플레이어가 이 구역에 들어왔다면?
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            SpawnWorker();
        }
    }

    void SpawnWorker()
    {
        if (workerNPC != null && !workerNPC.activeSelf)
        {
            workerNPC.SetActive(true); 
            Debug.Log("경고: 역무원이 스폰되었습니다.");

            if (spawnOnce)
            {
                gameObject.SetActive(false);
            }
        }
    }
}