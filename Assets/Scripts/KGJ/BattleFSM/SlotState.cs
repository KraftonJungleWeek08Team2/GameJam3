using System.Collections;

public class SlotState : ITurnState
{
    public void EnterState()
    {
        
        Managers.InputManager.SlotEnable(true); // InputManager의 액션 맵을 Slot으로 변경
        Managers.TurnManager.Player.Idle();     // Player의 애니메이션을 Idle로 변경
        Managers.TurnManager.CurrentEnemy.isMoving = false;

        if (Managers.TurnManager.CurrentEnemy != null)
        {
            Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
        }
        
        Managers.CameraManager.ChangeBattleCamera(true);    // 카메라를 전투 카메라로 변경
        Managers.TurnManager.SlotMachine.ShowSlotUI(Managers.TurnManager.IsFullCombo);      // 슬롯 머신 동작 시작
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.SlotMachine.HideSlotUI();  // 슬롯 머신 끄기
        Managers.InputManager.SlotEnable(false);        // InputManager의 액션 맵 구독 끊기
    }

    IEnumerator WaitOneFrame()
    {
        yield return null;
    }

    public void FixedUpdateState()
    {

    }
}
