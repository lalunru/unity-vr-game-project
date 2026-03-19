using UnityEngine;
using UnityEngine.UI;

public class UI_NoiseAnim : MonoBehaviour
{
    public float speed = 30f; // 흔들리는 속도
    private RawImage rawImage;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        if (rawImage != null)
        {
            // 이미지의 위치(UV 좌표)를 랜덤하게 계속 바꿈 -> 지지직 효과
            float randomX = Random.Range(0f, 100f);
            float randomY = Random.Range(0f, 100f);

            rawImage.uvRect = new Rect(randomX, randomY, 1, 1);
        }
    }
}