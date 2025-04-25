public class SlotState : ITurnState
{
    public void EnterState()
    {
        Managers.TurnManager.SlotMachine.OnSlotSuccessEvent += ChangeStateToAttack; // 액션 구독
        Managers.TurnManager.SlotMachine.OnSlotFailEvent += ChangeStateToKnockBack; // 액션 구독

        Managers.TurnManager.EnemyHpUI.ShowEnemyUI(); // 적 체력 UI 보이기
        Managers.InputManager.SlotEnable(true); // InputManager의 액션 맵을 Slot으로 변경
        Managers.TurnManager.Player.Idle();     // Player의 애니메이션을 Idle로 변경
        Managers.TurnManager.CurrentEnemy.isMoving = false;

        if (Managers.TurnManager.CurrentEnemy != null)
        {
            Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
        }
        
        Managers.CameraManager.ChangeBattleCamera(true);    // 카메라를 전투 카메라로 변경
        Managers.TurnManager.SlotMachine.ShowSlotUI();      // 슬롯 머신 동작 시작
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        SoundManager.Instance.PlaySlotResultSound();
        Managers.TurnManager.SlotMachine.HideSlotUI();  // 슬롯 머신 끄기
        Managers.InputManager.SlotEnable(false);        // InputManager의 액션 맵 구독 끊기

        Managers.TurnManager.SlotMachine.OnSlotSuccessEvent -= ChangeStateToAttack; // 성공 액션 해제
        Managers.TurnManager.SlotMachine.OnSlotFailEvent -= ChangeStateToKnockBack; // 실패 액션 해제
    }

    public void FixedUpdateState()
    {

    }

    void ChangeStateToAttack(SlotInfo slotInfo)
    {
        // TODO : 이곳에서 slotInfo에 따라 다른 ui 표출할 수 있음
        Managers.TurnManager.ChangeState(new AttackState(slotInfo));
    }

    void ChangeStateToKnockBack()
    {
        Managers.TurnManager.ChangeState(new KnockBackState());
    }
}
