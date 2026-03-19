using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [Header("설정")]
    public string gameSceneName = "GameScene"; // 넘어갈 게임 씬 이름
    public float delayTime = 1.0f; // 버튼 누르고 지연 시간 (소리 재생용)

    public void GameStart()
    {
        StartCoroutine(LoadSceneRoutine(gameSceneName));
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    }

    public void RestartGame()
    {
        // 현재 씬을 다시 로드 (재시작)
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(sceneName);
    }
}