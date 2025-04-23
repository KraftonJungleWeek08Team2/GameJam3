using UnityEngine;

public class AttackState : ITurnState
{
    // 나중에 Slotinfo로 전환
    SlotInfo _slotInfo;

    // RhythmAttack 받아오기
    // Player 받아오기

    public AttackState(SlotInfo slotInfo)
    {
        _slotInfo = slotInfo;
    }

    public void EnterState()
    {
        // RhythmAttack에 slotNum을 넘겨서 시작하기
        // Debug.Log($"{_slotInfo.GetValue(0)}, {_slotInfo.GetValue(1)}, {_slotInfo.GetValue(2)}");
    }
    public void ExecuteState()
    {
        // Code to execute during the attack state
    }
    public void ExitState()
    {
        // RhythmAttack 종료되었을 때 ChangeState(new MoveState) 실행되면서 실행됨
        // 풀콤보인지, 데미지 몇인지 받아오기
        // 적 hp를 CurrentEnemy에서 받아옴
        // 적이 이미 죽어있다면 적 처치를 여기서 해줌
        // 풀콤이 아니고 적 hp > 0 이면 Player 데미지 받고 넉백
        // 적 hp <= 0 이면 적 처치
    }
}