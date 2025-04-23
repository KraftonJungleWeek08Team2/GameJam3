using static UnityEditor.Experimental.GraphView.GraphView;

public class MoveState : ITurnState
{
    Player _player;
    Enemy _currentEnemy;

    public void EnterState()
    {
        Managers.CameraManager.ChangeBattleCamera(false); // 카메라를 MoveCamera로 변경
        _player = Managers.TurnManager.Player; // Player를 가져옴
        _currentEnemy = Managers.TurnManager.CurrentEnemy; // 현재 적을 가져옴
        _player.FakeRun(); // Player의 애니메이션을 FakeRun으로 변경

        // TODO : 세상을 움직임..
        
    }

    public void UpdateState()
    {
        // 플레이어와 적이 가까워지면 SlotState로 전환
        if (_player.transform.position.x + 2 <= _currentEnemy.transform.position.x)
        {
            Managers.TurnManager.ChangeState(new SlotState());
        }
    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {

    }
}
