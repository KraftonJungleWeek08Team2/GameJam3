public class TurnManager
{
    public ITurnState CurrentState => _currentState;
    private ITurnState _currentState;
    //public Enemy CurrentEnemy;

    public void Init()
    {
        _currentState = new MoveState();
    }

    public void ExecuteState()
    {
        CurrentState?.ExecuteState();
    }

    public void ChangeState(ITurnState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState?.EnterState();
    }
}
