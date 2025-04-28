using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMenuController : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button exitButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        howToPlayButton.onClick.AddListener(LoadHowToPlay1);
        exitButton.onClick.AddListener(ExitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene("MainScene_Normal");
    }

    void LoadHowToPlay1()
    {
        SceneManager.LoadScene("HowToPlay_1");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
