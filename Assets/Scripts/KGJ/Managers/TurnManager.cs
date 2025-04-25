using UnityEngine;

public class TurnManager
{
    public ITurnState CurrentState => _currentState;
    private ITurnState _currentState;

    public Enemy CurrentEnemy;
    public int CurrentEnemyIndex = 0; // 적 숫자
    public Player Player;

    public SlotMachineV2 SlotMachine => _slotMachine;
    SlotMachineV2 _slotMachine;

    public BeatBarPanelBehaviour BeatBarPanelBehaviour => _beatBarPanelBehaviour;
    BeatBarPanelBehaviour _beatBarPanelBehaviour;

    public ParallaxBackground ParallaxBackground => _parallaxBackground;
    ParallaxBackground _parallaxBackground;

    public EnemySpawner EnemySpawner => _enemySpawner;
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

    void OnApplicationQuit()
    {
        _currentState?.ExitState();
    }
}
