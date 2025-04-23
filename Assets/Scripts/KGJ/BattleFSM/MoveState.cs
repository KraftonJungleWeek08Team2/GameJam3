public class MoveState : ITurnState
{
    public void EnterState()
    {
        // Player 달림
        Managers.CameraManager.ChangeBattleCamera(false);
    }

    public void ExecuteState()
    {
    }

    public void ExitState()
    {
        // Player에서 검사한 Managers.TurnManager.CurrentEnemy를 갱신해주고 이 ExitState()를 호출해야함
        //Managers.TurnManager.ChangeState(new SlotState());
    }
}
