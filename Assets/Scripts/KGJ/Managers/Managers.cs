using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance => _instance;
    static Managers _instance;

    public static TurnManager TurnManager => _turnManager;
    static TurnManager _turnManager = new TurnManager();

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
    }

    void Update()
    {
        // TurnManager의 현재 상태를 실행
        _turnManager.ExecuteState();
    }
}
