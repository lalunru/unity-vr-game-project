using UnityEngine;

public class KeepBGM : MonoBehaviour
{
    private static KeepBGM instance;

    void Awake()
    {
        // 이미 BGM이 있으면 사라진다
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 내가 첫 BGM이라면 씬이 바껴도 파괴되지 않게 설정
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}