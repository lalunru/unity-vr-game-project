using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [Header("UI 연결")]
    public TMP_Text myText;

    [Header("오디오 설정")]
    public AudioSource audioSource; // 스피커
    public AudioClip typingLongSound; // 긴 타자 소리 파일

    [Header("내용 설정")]
    [TextArea]
    public string content = "> UPLOADING REPORT...\n> DATA SAVED.";

    [Header("추가 메시지")]
    public string askMessage = "\n\n> LOADING NEXT SHIFT AUTOMATICALLY...";

    public float typingSpeed = 0.05f;

    [Header("다음 씬 이름")]
    public string nextSceneName = "Day2";

    [Header("자동 대기 시간")]
    public float autoDelay = 3.0f;

    void Start()
    {
        if (myText == null) myText = GetComponent<TMP_Text>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (myText != null) StartCoroutine(ProcessSequence());
    }

    IEnumerator ProcessSequence()
    {
        myText.text = "";

        // 1. 소리 켜기
        PlaySoundLoop();

        // 2. 본문 출력
        foreach (char letter in content.ToCharArray())
        {
            myText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // 3. 소리 끄기
        StopSoundLoop();

        yield return new WaitForSeconds(0.5f);

        // 4. 소리 다시 켜기
        PlaySoundLoop();

        // 5. 안내 문구 출력
        foreach (char letter in askMessage.ToCharArray())
        {
            myText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // 6. 소리 완전히 끄기
        StopSoundLoop();

        // 7. 대기 후 이동
        Debug.Log(autoDelay + "초 뒤에 자동으로 넘어갑니다...");
        yield return new WaitForSeconds(autoDelay);

        LoadNextScene();
    }

    // 소리 켜는 함수
    void PlaySoundLoop()
    {
        if (audioSource != null && typingLongSound != null)
        {
            audioSource.clip = typingLongSound;
            audioSource.loop = true; // 무한 반복 설정
            audioSource.Play();
        }
    }

    // 소리 끄는 함수
    void StopSoundLoop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    void LoadNextScene()
    {
        Debug.Log("NEXT SCENE LOADING: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}