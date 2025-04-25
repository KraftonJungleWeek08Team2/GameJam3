public class MoveState : ITurnState
{
    public void EnterState()
    {
        Managers.CameraManager.ChangeBattleCamera(false); // 카메라를 MoveCamera로 변경
        Managers.TurnManager.Player.FakeRun(); // Player의 애니메이션을 FakeRun으로 변경
        Managers.TurnManager.CurrentEnemy.isMoving = true;
        Managers.TurnManager.ParallaxBackground.IsScrolling = true; // 배경 스크롤 시작
    }

    public void UpdateState()
    {
        // 플레이어와 적이 가까워지면 SlotState로 전환
        if (Managers.TurnManager.Player.transform.position.x + 2 > Managers.TurnManager.CurrentEnemy.transform.position.x)
        {
            Managers.TurnManager.ChangeState(new SlotState());
        }
    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.ParallaxBackground.IsScrolling = false;
    }
}
