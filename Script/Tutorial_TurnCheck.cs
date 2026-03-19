using UnityEngine;
using System.Collections;

public class Tutorial_TurnCheck : MonoBehaviour
{
    [Header("연결")]
    public Transform player;        // Final XR Origin
    public GameObject canvasObject; 

    [Header("다음 단계 연결")]
    public GameObject nextTutorialCanvas; // TutGrab

    [Header("설정")]
    public float turnThreshold = 15.0f;
    public float nextDelay = 3.0f;

    private Quaternion startRotation;
    private bool isFinished = false;

    void OnEnable()
    {
        if (player != null)
        {
            startRotation = player.rotation;
            isFinished = false;
        }
    }

    void Update()
    {
        if (isFinished || player == null) return;

        float angleDifference = Quaternion.Angle(player.rotation, startRotation);

        if (angleDifference > turnThreshold)
        {
            isFinished = true;

            if (canvasObject != null)
            {
                Canvas canvasComp = canvasObject.GetComponent<Canvas>();
                if (canvasComp != null) canvasComp.enabled = false; // 화면에서만 숨김

            }

            StartCoroutine(ActivateNextTutorial());
        }
    }

    IEnumerator ActivateNextTutorial()
    {
        yield return new WaitForSeconds(nextDelay);

        // 다음 튜토리얼 켜기
        if (nextTutorialCanvas != null)
        {
            nextTutorialCanvas.SetActive(true);
        }

        if (canvasObject != null) canvasObject.SetActive(false);
    }
}