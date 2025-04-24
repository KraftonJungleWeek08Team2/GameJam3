using UnityEngine;

public class GameManager
{
    public bool IsGameOver
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }
    bool _isGameOver = false;

    Canvas _gameOverCanvas;

    public void Init()
    {
        _gameOverCanvas = GameObject.FindAnyObjectByType<UI_GameOver>().GetComponent<Canvas>();
        _gameOverCanvas.enabled = false;
        IsGameOver = false;
    }

    public void GameOver()
    {
        IsGameOver = true;
        _gameOverCanvas.enabled = true;
        Managers.TurnManager.ChangeState(new GameoverState());
    }
}
