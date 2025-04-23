public class AttackState : ITurnState
{
    public void EnterState()
    {
        // RhythmAttack에 slotInfo을 넘겨서 시작하기
        // Debug.Log($"{_slotInfo.GetValue(0)}, {_slotInfo.GetValue(1)}, {_slotInfo.GetValue(2)}");

    }

    public void ExecuteState()
    {

    }

    public void ExitState()
    {
        if (Managers.TurnManager.CurrentEnemy.hp <= 0)
        {
            // 적의 hp가 0 이하라면 죽음 처리하고 MoveState로 전환
            Managers.CameraManager.RemoveMember(Managers.TurnManager.CurrentEnemy.transform);

            Managers.TurnManager.CurrentEnemy.Die();
            Managers.TurnManager.CurrentEnemy = null;
            Managers.TurnManager.ChangeState(new MoveState());
        }
        else
        {
            if (Managers.TurnManager.IsFullCombo)
            {
                // 풀 콤보라면, 다시 슬롯머신 상태로
                Managers.TurnManager.IsFullCombo = false;
                Managers.TurnManager.ChangeState(new SlotState());
            }
            else
            {
                // 풀 콤보가 아니라면, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
                Managers.TurnManager.ChangeState(new MoveState());
            }
        }
    }
}