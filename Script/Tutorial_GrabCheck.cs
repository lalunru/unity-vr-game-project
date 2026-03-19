using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Tutorial_GrabCheck : MonoBehaviour
{
    [Header("연결")]
    public GameObject grabTextCanvas;
    public XRGrabInteractable targetObject; // 손전등

    [Header("다음 단계")]
    public GameObject nextTutorialCanvas;

    private bool isFinished = false;

    void Update()
    {
        if (isFinished || targetObject == null) return;

        // 잡았는지 확인
        if (targetObject.isSelected)
        {
            isFinished = true;

            if (grabTextCanvas != null) grabTextCanvas.SetActive(false);

            StartCoroutine(ActivateNextTutorial());
        }
    }

    IEnumerator ActivateNextTutorial()
    {
        yield return new WaitForSeconds(1.0f); 

        if (nextTutorialCanvas != null)
        {
            nextTutorialCanvas.SetActive(true);
        }

        this.enabled = false;
    }
}