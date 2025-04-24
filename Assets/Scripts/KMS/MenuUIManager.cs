using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;

    [Header("How To Play UI")]
    [SerializeField] private GameObject howToPlayUI;    // HowToPlay 캔버스
    [SerializeField] private Image displayImage;        // 보여줄 이미지 컴포넌트
    [SerializeField] private Sprite[] howToPlayPages;   // 에디터에서 등록할 페이지 스프라이트 배열
    [SerializeField] private ToggleButton _toggleButton; //하드모드 토글로 사용할 예정
    private int currentPage;

    void Awake()
    {
        Instance = this;
        HideHowToPlay();
    }

    public void StartGame()
    {
        if(_toggleButton.IsOn())
            SceneManager.LoadScene("MainScene_Hard");
        else
            SceneManager.LoadScene("MainScene_Normal");
    }

    public void HowToPlayGame()
    {
        howToPlayUI.SetActive(true);
        currentPage = 0;
        displayImage.sprite = howToPlayPages[currentPage];
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Update()
    {
 
        if (!howToPlayUI.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextPage();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            PrevPage();
    }

    public void NextPage()
    {
        if (currentPage < howToPlayPages.Length - 1)
        {
            currentPage++;
            displayImage.sprite = howToPlayPages[currentPage];
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            displayImage.sprite = howToPlayPages[currentPage];
        }
    }

    // 백 버튼이나 다른 UI 이벤트에서 호출
    public void HideHowToPlay()
    {
        howToPlayUI.SetActive(false);
    }
    

}
