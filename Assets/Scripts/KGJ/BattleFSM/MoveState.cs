public class MoveState : ITurnState
{
    public void EnterState()
    {
        Managers.CameraManager.ChangeBattleCamera(false); // 카메라를 MoveCamera로 변경
        Managers.TurnManager.Player.Run(); // Player의 애니메이션을 Run으로 변경
        
    }

    public void ExecuteState()
    {
    }

    public void ExitState()
    {

    }
}
