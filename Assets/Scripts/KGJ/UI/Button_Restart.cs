using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button_Restart : MonoBehaviour
{
    Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Restart);
    }

    void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
