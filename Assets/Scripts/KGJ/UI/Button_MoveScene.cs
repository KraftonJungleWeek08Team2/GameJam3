using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_MoveScene : MonoBehaviour
{
    [SerializeField] bool isNext;
    int currentSceneIndex;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (isNext)
        {
            button.onClick.AddListener(LoadNextScene);
        }
        else
        {
            button.onClick.AddListener(LoadPrevScene);
        }
    }

    void LoadPrevScene()
    {
        
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
