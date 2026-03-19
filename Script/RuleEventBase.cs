using UnityEngine;

public abstract class RuleEventBase : MonoBehaviour
{
    [Header("Rule Settings")]
    public int myRuleIndex = 0;

    // 이 규칙이 성공적으로 완료되었는지 여부
    protected bool isRuleCompleted = false;

    // GameManager가 이 규칙을 시작시킬 때 호출하는 공통 함수
    public abstract void StartRule();

    // 규칙이 성공적으로 끝났을 때 호출하는 공통 함수
    protected void CompleteRule()
    {
        if (isRuleCompleted) return; // 중복 호출 방지
        isRuleCompleted = true;

        Debug.Log("규칙 수행 성공! (ID: " + (myRuleIndex + 1) + ")"); // 디버깅 편의를 위해 ID 표시

        GameManager.Instance.NotifyRuleCompleted(myRuleIndex);
    }

    // 규칙을 실패했을 때 호출하는 공통 함수
    protected void FailRule()
    {
        if (isRuleCompleted) return;
        Debug.Log("규칙 수행 실패!");

        GameManager.Instance.FailRule();
    }
}