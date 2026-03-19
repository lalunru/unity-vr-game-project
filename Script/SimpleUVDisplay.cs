using UnityEngine;
using System.Collections;

public class SimpleUVDisplay : MonoBehaviour
{
    [Header("감시할 대상")]
    public GameObject uvLightObject; 

    [Header("화면 교체")]
    public GameObject normalScreen; // 평소 화면 

    [Header("딜레이 설정")]
    public float delayTime = 1.5f;

    private bool isRevealed = false;
    private Coroutine revealCoroutine;

    void Update()
    {
        if (uvLightObject == null) return;

        // 1. UV 라이트가 켜지면 일정 시간 후에 화면 교체
        if (uvLightObject.activeInHierarchy)
        {
            if (!isRevealed && revealCoroutine == null)
            {
                revealCoroutine = StartCoroutine(ShowHintRoutine());
            }
        }
        else
        {
            // 2. 꺼지면 즉시 원상복구
            ResetScreen();
        }
    }

    IEnumerator ShowHintRoutine()
    {
        yield return new WaitForSeconds(delayTime);

        if (normalScreen != null) normalScreen.SetActive(false);

        isRevealed = true;
    }

    void ResetScreen()
    {
        if (revealCoroutine != null) StopCoroutine(revealCoroutine);
        revealCoroutine = null;

        if (normalScreen != null) normalScreen.SetActive(true);

        isRevealed = false;
    }
}