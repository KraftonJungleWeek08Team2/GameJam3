using UnityEngine;

public class TurnManager
{
    public ITurnState CurrentState => _currentState;
    private ITurnState _currentState;

    public Enemy CurrentEnemy;
    public int CombinationFailCount = 0; // 룰렛에서 조합 안나온 횟수
    public Player Player;

    public SlotMachineV2 SlotMachine => _slotMachine;
    SlotMachineV2 _slotMachine;

    public BeatBarSystem BeatBarSystem => _beatBarSystem;
    BeatBarSystem _beatBarSystem;

    public ParallaxBackground ParallaxBackground => _parallaxBackground;
    ParallaxBackground _parallaxBackground;

    public StageManager StageManager => _stageManager;
    StageManager _stageManager;

    public UI_EnemyHp EnemyHpUI => _enemyHpUI;
    UI_EnemyHp _enemyHpUI;

    public FeverTimeController FeverTimeController => _feverTimeController;
    FeverTimeController _feverTimeController;

    public SkillManager SkillManager => _skillManager;
    SkillManager _skillManager;

    public SkillBook SkillBook => _skillBook;
    SkillBook _skillBook;

    public void Init()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        _slotMachine = GameObject.FindAnyObjectByType<SlotMachineV2>();
        _beatBarSystem = GameObject.FindAnyObjectByType<BeatBarSystem>();
        _parallaxBackground = GameObject.FindAnyObjectByType<ParallaxBackground>();
        _stageManager = GameObject.FindAnyObjectByType<StageManager>();
        _enemyHpUI = GameObject.FindAnyObjectByType<UI_EnemyHp>();
        _feverTimeController = GameObject.FindAnyObjectByType<FeverTimeController>();
        _skillManager = GameObject.FindAnyObjectByType<SkillManager>();
        _skillBook = new SkillBook();
        _stageManager.SpawnNext();
        CombinationFailCount = 0;

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
