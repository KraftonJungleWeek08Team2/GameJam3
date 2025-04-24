using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance => _instance;
    static Managers _instance;

    public static GameManager GameManager => _gameManager;
    static GameManager _gameManager = new GameManager();

    public static TurnManager TurnManager => _turnManager;
    static TurnManager _turnManager = new TurnManager();

    public static CameraManager CameraManager => _cameraManager;
    static CameraManager _cameraManager = new CameraManager();

    public static InputManager InputManager => _inputManager;
    static InputManager _inputManager = new InputManager();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitManagers()
    {
        CameraManager.Init();
        TurnManager.Init();
        InputManager.Init();
        GameManager.Init();
    }

    void Update()
    {
        _turnManager.UpdateState();
    }

    void FixedUpdate()
    {
        _turnManager.FixedUpdateState();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitManagers();
    }


    void OnDestroy()
    {
        if (_instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
