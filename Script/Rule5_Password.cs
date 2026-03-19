using UnityEngine;

public class Rule5_Password : RuleEventBase
{
    public override void StartRule()
    {
        Debug.Log("규칙 5: 비밀번호 입력 시작");
    }

    // 키패드 정답을 맞췄을 때 외부에서 호출할 함수
    public void SolvePassword()
    {
        Debug.Log("비밀번호 정답! 문을 엽니다.");

        // 1. 규칙 완료 처리
        CompleteRule();
    }
}