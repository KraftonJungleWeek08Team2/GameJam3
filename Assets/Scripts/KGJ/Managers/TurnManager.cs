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

    public BeatBarPanelBehaviour BeatBarPanelBehaviour => _beatBarPanelBehaviour;
    BeatBarPanelBehaviour _beatBarPanelBehaviour;

    public void Init()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        _slotMachine = GameObject.FindAnyObjectByType<SlotMachine>();
        _beatBarPanelBehaviour = GameObject.FindAnyObjectByType<BeatBarPanelBehaviour>();

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

    public void EndSlotState()
    {
        // SlotMachine 결과를 넘겨줌
        if (IsSlotSuccess())
        {
            ChangeState(new AttackState());
        }
        else
        {
            // 슬롯머신 실패시, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
            // TODO : 현재 적의 공격력만큼 데미지를 주어야함
            Player.TakeDamage(1);
            ChangeState(new MoveState());
        }
    }

    public void StartSlotState()
    {
        ChangeState(new SlotState());
    }

    public void EndAttackState()
    {
        if (CurrentEnemy.hp <= 0)
        {
            // 적의 hp가 0 이하라면 죽음 처리하고 MoveState로 전환
            Managers.CameraManager.RemoveMember(CurrentEnemy.transform);

            CurrentEnemy.Die();
            CurrentEnemy = null;
            ChangeState(new MoveState());
        }
        else
        {
            if (IsFullCombo)
            {
                // 풀 콤보라면, 다시 슬롯머신 상태로
                IsFullCombo = false;
                ChangeState(new SlotState());
            }
            else
            {
                // 풀 콤보가 아니라면, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
                ChangeState(new MoveState());
            }
        }
    }

    bool IsSlotSuccess()
    {
        SlotInfo slotInfo = SlotMachine.SlotInfo;
        for (int i = 0; i < slotInfo.SlotCount; i++)
        {
            if (slotInfo.GetValue(i) == 0)
                return false;
        }
        return true;
    }
}
