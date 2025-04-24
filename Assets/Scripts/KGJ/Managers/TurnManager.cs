using UnityEngine;

public class TurnManager
{
    public ITurnState CurrentState => _currentState;
    private ITurnState _currentState;

    public Enemy CurrentEnemy;
    public int CurrentEnemyIndex = 0; // 적 숫자
    public Player Player;
    public bool IsFullCombo = false;

    public SlotMachineV2 SlotMachine => _slotMachine;
    SlotMachineV2 _slotMachine;

    public BeatBarPanelBehaviour BeatBarPanelBehaviour => _beatBarPanelBehaviour;
    BeatBarPanelBehaviour _beatBarPanelBehaviour;

    public ParallaxBackground ParallaxBackground => _parallaxBackground;
    ParallaxBackground _parallaxBackground;

    EnemySpawner _enemySpawner;

    public UI_EnemyHp EnemyHpUI => _enemyHpUI;
    UI_EnemyHp _enemyHpUI;

    public void Init()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        _slotMachine = GameObject.FindAnyObjectByType<SlotMachineV2>();
        _beatBarPanelBehaviour = GameObject.FindAnyObjectByType<BeatBarPanelBehaviour>();
        _parallaxBackground = GameObject.FindAnyObjectByType<ParallaxBackground>();
        _enemySpawner = GameObject.FindAnyObjectByType<EnemySpawner>();
        _enemyHpUI = GameObject.FindAnyObjectByType<UI_EnemyHp>();
        if (_enemyHpUI == null)
        {
            Debug.Log("[KGJ] EnemyHpUI not found");
        }
        CurrentEnemyIndex = 0;

        _currentState = new MoveState();
    }

    public void UpdateState()
    {
        if (!Managers.GameManager.IsGameOver)
            CurrentState?.UpdateState();
    }

    public void FixedUpdateState()
    {
        if (!Managers.GameManager.IsGameOver)
            CurrentState?.FixedUpdateState();
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
            ChangeState(new KnockBackState());
        }
    }

    public void EndAttackState()
    {
        if (CurrentEnemy.hp <= 0)
        {
            Managers.TurnManager.EnemyHpUI.HideEnemyUI(); // 적 체력 UI 숨기기
            Managers.CameraManager.RemoveMember(CurrentEnemy.transform);
            CurrentEnemy.Die();

            // EnemySpawner에서 적을 생성하고 CurrentEnemy에 넣어줌
            CurrentEnemy = _enemySpawner.Spawn();
            CurrentEnemyIndex++;
            Managers.CameraManager.AddMember(CurrentEnemy.transform, 0.5f, 1f);
            ChangeState(new MoveState());
            
        }
        else
        {
            if (IsFullCombo)
            {
                // 풀 콤보라면, 다시 슬롯머신 상태로
                // 원 모어 UI를 띄우고, 슬롯머신 시간도 1초 줄임
                ChangeState(new SlotState());
                IsFullCombo = false;
            }
            else
            {
                // 풀 콤보가 아니라면, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
                ChangeState(new KnockBackState());
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
