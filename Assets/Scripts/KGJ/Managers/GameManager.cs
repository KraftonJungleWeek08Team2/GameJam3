public class GameManager
{
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    bool _isGameOver = false;

    public void GameOver()
    {
        IsGameOver = true;
        // 게임 오버 UI 활성화
        // 게임 종료 처리
    }
}
