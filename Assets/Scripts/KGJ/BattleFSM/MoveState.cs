public class MoveState : ITurnState
{
    public void EnterState()
    {
        Managers.CameraManager.ChangeBattleCamera(false); // 카메라를 MoveCamera로 변경
        Managers.TurnManager.Player.FakeRun(); // Player의 애니메이션을 FakeRun으로 변경
        
    }

    public void UpdateState()
    {
    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {

    }
}
