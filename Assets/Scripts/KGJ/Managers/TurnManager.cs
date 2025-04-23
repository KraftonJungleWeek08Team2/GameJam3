using UnityEngine;

public class TurnManager
{
    public ITurnState CurrentState => _currentState;
    private ITurnState _currentState;

    public Enemy CurrentEnemy;
    public Player Player;
    public bool IsFullCombo = false;

    public SlotMachine SlotMachine => _slotMachine;
    SlotMachine _slotMachine;

    public void Init()
    {
        _slotMachine = GameObject.FindAnyObjectByType<SlotMachine>();
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

    public void EndAttackState()
    {
        
    }
}
