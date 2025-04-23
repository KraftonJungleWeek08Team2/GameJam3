using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void HowtoplayGame() 
    {
        // TODO: 금지씨 게임설명을 넣어주세요
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
