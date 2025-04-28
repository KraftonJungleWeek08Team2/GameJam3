using UnityEngine;

public class GameManager
{
    public bool IsGameOver
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }
    bool _isGameOver = false;

    public bool IsStageClear
    {
        get { return _isStageClear; }
        set { _isStageClear = value; }
    }
    bool _isStageClear = false;

    Canvas _gameOverCanvas;

    public void Init()
    {
        _gameOverCanvas = GameObject.FindAnyObjectByType<UI_GameOver>().GetComponent<Canvas>();
        _gameOverCanvas.enabled = false;
        IsGameOver = false;
        IsStageClear = false;
    }

    public void StageClear()
    {
        IsStageClear = true;
        Managers.TurnManager.ChangeState(new StageClearState());
    }

    public void GameOver()
    {
        IsGameOver = true;
        _gameOverCanvas.enabled = true;
        Managers.TurnManager.ChangeState(new GameoverState());
    }
}
