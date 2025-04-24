using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button_Restart : MonoBehaviour
{
    Button _button;
    public int sceneIndex;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Restart);
    }

    void Restart()
    {
        if (sceneIndex == 1)
        {
            SceneManager.LoadScene("MainScene_Normal");
        }
        else if (sceneIndex == 2)
        {
            SceneManager.LoadScene("MainScene_Hard");
        }
    }
}
