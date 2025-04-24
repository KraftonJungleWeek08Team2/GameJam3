using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    [Header("색상 설정")]
    [SerializeField] private Color offColor = Color.white;
    [SerializeField] private Color onColor = Color.green;

    private Button button;
    private Image image;
    private bool isOn = false;

    void Awake()
    {
        button = GetComponent<Button>();
        image = button.GetComponent<Image>();

        // 초기 상태
        image.color = offColor;

        // 버튼 클릭 시 ToggleState 호출
        button.onClick.AddListener(ToggleState);
    }

    void ToggleState()
    {
        isOn = !isOn;
        image.color = isOn ? onColor : offColor;
        Debug.Log($"토글 상태: {isOn}");
    }

    /// <summary>
    /// 외부에서 현재 상태를 조회하고 싶을 때
    /// </summary>
    public bool IsOn() => isOn;
}
