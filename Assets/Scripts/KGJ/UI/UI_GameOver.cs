using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    // TODO : 마저    구현하기
    GameObject _gameOverPanel;
    CanvasGroup _canvasGroup;
    Button _restartButton;

    void Start()
    {
        _gameOverPanel = transform.GetChild(0).gameObject;
        _canvasGroup = _gameOverPanel.GetComponent<CanvasGroup>();
        _restartButton = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        Managers.GameManager.OnGameOverEvent += OnGameOver;
    }

    void OnGameOver()
    {
        
    }
}
