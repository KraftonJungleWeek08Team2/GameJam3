using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance => _instance;
    static Managers _instance;

    public static GameManager GameManager => _gameManager;
    static GameManager _gameManager = new GameManager();

    public static TurnManager TurnManager => _turnManager;
    static TurnManager _turnManager = new TurnManager();

    public static TargetGroupManager TargetGroupManager => _targetGroupManager;
    static TargetGroupManager _targetGroupManager = new TargetGroupManager();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        TurnManager.Init();
        TargetGroupManager.Init();
    }

    void Update()
    {
        // TurnManager의 현재 상태를 실행
        _turnManager.ExecuteState();
    }
}
