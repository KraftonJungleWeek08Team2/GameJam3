public class SlotState : ITurnState
{
    public void EnterState()
    {
        // SlotMachine 시작함
        Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
        Managers.CameraManager.ChangeBattleCamera(true);

        Managers.TurnManager.SlotMachine.ShowSlotUI();
    }

    public void ExecuteState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.SlotMachine.HideSlotUI();
        // SlotMachine 결과를 넘겨줌
        if (IsSlotSuccess())
        {
            Managers.TurnManager.ChangeState(new AttackState());
        }
        else
        {
            // 슬롯머신 실패시, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
            // TODO : 현재 적의 공격력만큼 데미지를 주어야함
            Managers.TurnManager.Player.TakeDamage(1);
            Managers.TurnManager.ChangeState(new MoveState());
        }
        
    }

    bool IsSlotSuccess()
    {
        SlotInfo slotInfo = Managers.TurnManager.SlotMachine.SlotInfo;
        for (int i = 0; i < slotInfo.SlotCount; i++)
        {
            if (slotInfo.GetValue(i) == 0)
                return false;
        }
        return true;
    }
}
